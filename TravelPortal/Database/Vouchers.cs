using System;
using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public class Vouchers
    {
        public static List<Voucher> GetAll()
        {
            try
            {
                return ExecuteQuery(Queries.Vouchers.SelectAllFunction);
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при получении списка путёвок", e);
            }
        }

        public static List<Voucher> SearchByClientFio(string clientFio)
        {
            try
            {
                return ExecuteQuery(Queries.Vouchers.Search(clientFio));
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при получении списка путёвок", e);
            }
        }

        public static List<Voucher> SearchByStatus(string status)
        {
            try
            {
                return ExecuteQuery(Queries.Vouchers.FilterStatus(status));
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при получении списка путёвок", e);
            }
        }

        private static List<Voucher> ExecuteQuery(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return new List<Voucher>();
                        List<Voucher> vouchers = new List<Voucher>();
                        while (reader.Read())
                        {
                            int voucherId = reader.GetInt32(0);
                            string fio = reader.GetString(1).TrimEnd();
                            string hotel = reader.GetString(2).TrimEnd();
                            string from = reader.GetString(3).TrimEnd();
                            string to = reader.GetString(4).TrimEnd();
                            string phone = reader.GetString(5).TrimEnd();
                            string address = reader.GetString(6).TrimEnd();
                            NpgsqlDate birthday = reader.GetDate(7);

                            vouchers.Add(new Voucher(voucherId, fio, from, to, hotel,
                                address, phone, birthday));
                        }

                        return vouchers;
                    }
                }
            }
        }
    }
}