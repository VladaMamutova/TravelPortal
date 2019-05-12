using System;
using System.Collections.ObjectModel;
using NpgsqlTypes;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    public class Routes
    {
        public static ObservableCollection<Route> GetAll()
        {
            try
            {
                return MainTables.GetRoutes(Queries.GetRoutes());
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
                return MainTables.GetRoutes(Queries.Routes.Search(new NpgsqlDate(date)));
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
                return MainTables.GetRoutes(Queries.Routes.Search(duration));
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
                return MainTables.GetRoutes(Queries.Routes.FilterHotel(residence));
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
                return MainTables.GetRoutes(Queries.Routes.FilterTransport(transport));
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Произошла ошибка при фильтрации записей по типу " +
                    "транспорта маршрута", e);
            }
        }
    }
}