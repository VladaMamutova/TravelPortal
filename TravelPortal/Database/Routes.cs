using System;
using System.Collections.ObjectModel;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public class Routes
    {
        public static ObservableCollection<Route> GetRoutes()
        {
            try
            {
                return ExecuteQuery(Queries.SelectRouteView);
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при получении списка маршрутов.", e);
            }
        }

        public static ObservableCollection<Route> SearchByDate(DateTime date)
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

        public static ObservableCollection<Route> SearchByDuration(int duration)
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

        public static ObservableCollection<Route> FilterResidence(string residence)
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

        public static ObservableCollection<Route> FilterTransport(string transport)
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

        private static ObservableCollection<Route> ExecuteQuery(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return new ObservableCollection<Route>();
                        ObservableCollection<Route> routes = new ObservableCollection<Route>();
                        while (reader.Read())
                        {
                            int routeId = reader.GetInt32(0);
                            string hotel = reader.GetString(1);
                            string from = reader.GetString(2);
                            string to = reader.GetString(3);
                            double price = reader.GetDouble(4);
                            NpgsqlDate date = reader.GetDate(5);
                            int duration = reader.GetInt32(6);
                            bool meels = reader.GetBoolean(7);
                            string transport = reader.GetString(8);
                            double transportPrice = reader.GetDouble(9);

                            routes.Add(new Route(routeId, hotel, from, to,
                                price,
                                new DateTime(date.Year, date.Month, date.Day),
                                duration, meels, transport, transportPrice));
                        }

                        return routes;
                    }
                }
            }
        }
    }
}