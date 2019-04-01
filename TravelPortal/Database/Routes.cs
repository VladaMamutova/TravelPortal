using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Entities;

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
                    "Произошла ошибка при получении списка маршрутов", e);
            }
        }

        public static List<Route> Search(string routeName)
        {
            try
            {
                return ExecuteQuery(Queries.Routes.FilterName(routeName));
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при фильтрации записей по названию маршрута",
                    e);
            }

        }

        public static List<Route> ExecuteQuery(string query)
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
                            string name = reader.GetString(1).TrimEnd();
                            double cost = reader.GetDouble(2);
                            NpgsqlDate date = reader.GetDate(3);
                            int duration = reader.GetInt32(4);

                            routes.Add(new Route(routeId, name, date, duration,
                                cost));
                            //int routeId = reader.GetInt32(0);
                            //string name = reader.GetString(1).TrimEnd();
                            //NpgsqlDate date = reader.GetDate(2);
                            //int transportTypeId = reader.GetInt32(3);
                            //int residenceId = reader.GetInt32(4);
                            //bool meels = reader.GetBoolean(5);
                            //int duration = reader.GetInt32(6);
                            //double cost = reader.GetDouble(7);
                            //int agencyId = reader.GetInt32(8);

                            //routes.Add(new Route(routeId, residenceId,
                            //    transportTypeId, agencyId, name, date,
                            //    meels, duration, cost));
                        }

                        return routes;
                    }
                }
            }
        }
    }
}