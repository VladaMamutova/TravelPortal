using System;
using System.Collections.ObjectModel;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public static class Customers
    {

        public static ObservableCollection<Customer> GetCustomers()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                using (var command = new NpgsqlCommand(
                    Queries.SelectCustomerView, connection))
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

        public static ObservableCollection<Employee> GetEmployees()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                using (var command = new NpgsqlCommand(
                    Queries.SelectEmployeeView, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<Employee> collection =
                            new ObservableCollection<Employee>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string agency = reader.GetString(1);
                            string city = reader.GetString(2);
                            string name = reader.GetString(3);
                            string login = reader.GetString(4);


                            collection.Add(new Employee(id, agency, city, name, login));
                        }

                        return collection;
                    }
                }
            }
        }
    }
}