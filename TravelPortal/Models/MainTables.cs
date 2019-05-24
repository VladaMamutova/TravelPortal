using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Models
{
    public static class MainTables
    {
        private static readonly Configuration _configuration;

        static MainTables()
        {
            _configuration = Configuration.GetConfiguration();
        }

        public static void Execute(string query)
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command =
                    new NpgsqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteAddUpdateQuery(string query)
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command =
                    new NpgsqlCommand(query, connection))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        public static ObservableCollection<Route> GetRoutes(string query)
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return new ObservableCollection<Route>();
                        ObservableCollection<Route> routes =
                            new ObservableCollection<Route>();
                        while (reader.Read())
                        {
                            int routeId = reader.GetInt32(0);
                            string hotel = reader.GetString(1);
                            string from = reader.GetString(2);
                            string to = reader.GetString(3);
                            NpgsqlDate date = reader.GetDate(4);
                            int duration = reader.GetInt32(5);
                            bool meels = reader.GetBoolean(6);
                            string transport = reader.GetString(7);
                            double hotelPrice = reader.GetDouble(8);
                            double transportPrice = reader.GetDouble(9);

                            routes.Add(new Route(routeId, hotel, from, to,
                                new DateTime(date.Year, date.Month, date.Day),
                                duration, meels, transport, hotelPrice + transportPrice,
                                transportPrice, transportPrice));
                        }

                        return routes;
                    }
                }
            }
        }

        public static ObservableCollection<Voucher> GetVouchers(string query)
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return new ObservableCollection<Voucher>();
                        ObservableCollection<Voucher> vouchers =
                            new ObservableCollection<Voucher>();
                        while (reader.Read())
                        {
                            int voucherId = reader.GetInt32(0);
                            int routeId = reader.GetInt32(1);
                            string hotel = reader.GetString(2);
                            NpgsqlDate date = reader.GetDate(3);
                            int duration = reader.GetInt32(4);
                            double fullPrice = reader.GetDouble(5);
                            string fio = reader.GetString(6);
                            string phone = reader.GetString(7);


                            vouchers.Add(new Voucher(voucherId, routeId, hotel,
                                new DateTime(date.Year, date.Month, date.Day),
                                duration, fullPrice,
                                fio, phone));
                        }

                        return vouchers;
                    }
                }
            }
        }

        public static ObservableCollection<Customer> GetCustomers()
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.GetCustomers(), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<Customer> collection =
                            new ObservableCollection<Customer>();
                        while (reader.Read())
                        {
                            int voucherCount = reader.GetInt32(0);
                            string fio = reader.GetString(1);
                            string phone = reader.GetString(2);
                            string address = reader.GetString(3);
                            NpgsqlDate birthday = reader.GetDate(4);
                            string status = reader.GetString(5);


                            collection.Add(new Customer(voucherCount, fio,
                                phone, address,
                                new DateTime(birthday.Year, birthday.Month,
                                    birthday.Day), status));
                        }

                        return collection;
                    }
                }
            }
        }

        public static ObservableCollection<User> GetUsers()
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.SelectAll(DictionaryKind.User), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<User> collection =
                            new ObservableCollection<User>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int role = reader.GetInt32(1);
                            string name = reader.GetString(2);
                            string login = reader.GetString(3);
                            int agencyId = reader.IsDBNull(4)
                                ? -1
                                : reader.GetInt32(4);
                            string agency = reader.IsDBNull(5)
                                ? ""
                                : reader.GetString(5);
                            string city = reader.IsDBNull(6)
                                ? ""
                                : reader.GetString(6);

                            collection.Add(new User(id, (Roles) role, name,
                                login, agencyId, agency, city));
                        }

                        return collection;
                    }
                }
            }
        }

        public static User GetCurrentUser(string login)
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.GetUser(login), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;

                        if (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            int role = reader.GetInt32(1);
                            string name = reader.GetString(2);
                            int agencyId = reader.IsDBNull(3)
                                ? -1
                                : reader.GetInt32(3);
                            string agency = reader.IsDBNull(4)
                                ? ""
                                : reader.GetString(4);
                            string city = reader.IsDBNull(5)
                                ? ""
                                : reader.GetString(5);

                            return new User(id, (Roles) role, name, login,
                                agencyId, agency, city);
                        }

                        return null;
                    }
                }
            }
        }

        public static IList<RowDataItem> GetRatingCollection(string query,
            ref IList<string> headers)
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return new List<RowDataItem>();

                        headers.Clear();
                        List<RowDataItem> rowDataItems =
                            new List<RowDataItem>();

                        for (int i = 0; i < reader.FieldCount; i++)
                            headers.Add(reader.GetName(i));

                        while (reader.Read())
                        {
                            List<string> rowFields = new List<string>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object value = reader.GetValue(i);
                                switch (value)
                                {
                                    case int intValue:
                                        rowFields.Add(intValue.ToString("N0"));
                                        break;
                                    case long longValue:
                                        rowFields.Add(longValue.ToString("N0"));
                                        break;
                                    case decimal decimalValue:
                                        rowFields.Add(
                                            decimalValue.ToString("N2"));
                                        break;
                                    case DateTime dateTimeValue:
                                        rowFields.Add(dateTimeValue
                                            .ToShortDateString());
                                        break;
                                    default:
                                        rowFields.Add(value.ToString());
                                        break;
                                }
                            }

                            rowDataItems.Add(new RowDataItem(rowFields));
                        }

                        return rowDataItems;
                    }
                }
            }
        }

        public static List<HotelRank> GetHotelRankCollection()
        {
            using (var connection =
                new NpgsqlConnection(_configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.RankHotels(), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        List<HotelRank> collection =
                            new List<HotelRank>();
                        while (reader.Read())
                        {
                            string name = reader.GetString(0);
                            string city = reader.GetString(1);
                            int type = reader.GetInt32(2);
                            int voucherCount = reader.GetInt32(3);
                            double popularity = reader.GetDouble(4);

                            collection.Add(new HotelRank(name, city, type,
                                voucherCount, popularity));
                        }

                        return collection;
                    }
                }
            }
        }

        public static void SaveHotelsRankToExcel(List<HotelRank> collection)
        {
            // Создаём объект приложения Excel и новый рабочий лист.
            Microsoft.Office.Interop.Excel.Application excelApp =
                new Microsoft.Office.Interop.Excel.Application();
            excelApp.Application.Workbooks.Add(Type.Missing);
            
            // Подписываем заголовки столбцов и вычисляем ширину ячейки так,
            // так чтобы все значения в неё вмещались без обрезки.
            excelApp.Cells[1, 1] = "Название отеля";
            excelApp.Columns[1].ColumnWidth = collection.Max(hotel => hotel.Name.Length) + 2;
            excelApp.Cells[1, 2] = "Город";
            excelApp.Columns[2].ColumnWidth = collection.Max(hotel => hotel.City.Length) + 2;
            excelApp.Cells[1, 3] = "Категория (кол-во звёзд)";
            excelApp.Columns[3].ColumnWidth = 23;
            excelApp.Cells[1, 4] = "Количество путёвок (шт.)";
            excelApp.Columns[4].ColumnWidth = 23;
            excelApp.Cells[1, 5] = "Популярность (%)";
            excelApp.Columns[5].ColumnWidth = 17;

            // Выводим данные.
            for (int i = 0; i < collection.Count; i++)
            {
                excelApp.Cells[i + 2, 1] = collection[i].Name;
                excelApp.Cells[i + 2, 2] = collection[i].City;
                excelApp.Cells[i + 2, 3] = collection[i].Type;
                excelApp.Cells[i + 2, 4] =
                    collection[i].VoucherClount.ToString();
                excelApp.Cells[i + 2, 5] = collection[i].Popularity
                    .ToString(CultureInfo.InvariantCulture);
            }

            // Для отображения полученного результата, показываем документ.
            excelApp.Visible = true;
        }
    }
}