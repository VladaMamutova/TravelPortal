using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public class Routes
    {
        public static List<Route> GetAll()
        {
            try
            {
                return ExecuteQuery(Queries.Routes.SelectAll);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при получении списка маршрутов.", e);
            }
        }

        public static List<Route> SearchByDate(DateTime date)
        {
            try
            {
                return ExecuteQuery(Queries.Routes.Search(new NpgsqlDate(date)));
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при фильтрации записей по дате начала " +
                    "маршрута.", e);
            }
        }

        public static List<Route> SearchByDuration(int duration)
        {
            try
            {
                return ExecuteQuery(Queries.Routes.Search(duration));
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при фильтрации записей по длительности " +
                    "маршрута.", e);
            }
        }

        public static List<Route> FilterResidence(string residence)
        {
            try
            {
                return ExecuteQuery(Queries.Routes.FilterHotel(residence));
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при фильтрации записей по отелю " +
                    "маршрута", e);
            }
        }

        public static List<Route> FilterTransport(string transport)
        {
            try
            {
                return ExecuteQuery(Queries.Routes.FilterTransport(transport));
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при фильтрации записей по типу " +
                    "транспорта маршрута", e);
            }
        }

        private static List<Route> ExecuteQuery(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return new List<Route>();
                        List<Route> routes = new List<Route>();
                        while (reader.Read())
                        {
                            int routeId = reader.GetInt32(0);
                            string from = reader.GetString(1).TrimEnd();
                            string to = reader.GetString(2).TrimEnd();
                            double cost = reader.GetDouble(3);
                            NpgsqlDate date = reader.GetDate(4);
                            int duration = reader.GetInt32(5);
                            string residence = reader.GetString(6).TrimEnd();
                            bool meels = reader.GetBoolean(7);
                            string transport = reader.GetString(8).TrimEnd();
                            double transportCost = reader.GetDouble(9);

                            routes.Add(new Route(routeId, from, to, date,
                                duration, cost, residence, meels, transport,
                                transportCost));
                        }

                        return routes;
                    }
                }
            }
        }
    }
}