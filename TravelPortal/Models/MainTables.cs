using System;
using System.Collections.ObjectModel;
using System.Xml.Schema;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    public static class MainTables
    {
        public static ObservableCollection<Route> GetRoutes(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
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

        public static ObservableCollection<Voucher> GetVouchers(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return new ObservableCollection<Voucher>();
                        ObservableCollection<Voucher> vouchers = new ObservableCollection<Voucher>();
                        while (reader.Read())
                        {
                            int voucherId = reader.GetInt32(0);
                            string hotel = reader.GetString(1);
                            NpgsqlDate date = reader.GetDate(2);
                            int duration = reader.GetInt32(3);
                            double fullPrice = reader.GetDouble(4);
                            string fio = reader.GetString(5);
                            string phone = reader.GetString(6);


                            vouchers.Add(new Voucher(voucherId, hotel,
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
                new NpgsqlConnection(Configuration.ConnectionString))
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
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.SelectUserView, connection))
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
                            int agencyId = reader.IsDBNull(4) ? -1 : reader.GetInt32(4);
                            string agency = reader.IsDBNull(5) ? "" : reader.GetString(5);
                            string city = reader.IsDBNull(6) ? "" : reader.GetString(6);

                            collection.Add(new User(id, (Roles)role, name, login, agencyId, agency, city));
                        }

                        return collection;
                    }
                }
            }
        }

        public static User GetCurrentUser(string login)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.SelectUser(login), connection))
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
                            int agencyId = reader.IsDBNull(3) ? -1 : reader.GetInt32(3);
                            string agency = reader.IsDBNull(4) ? "" : reader.GetString(4);
                            string city = reader.IsDBNull(5) ? "" : reader.GetString(5);

                            return new User(id, (Roles)role, name, login, agencyId, agency, city);
                        }

                        return null;
                    }
                }
            }
        }
    }
}