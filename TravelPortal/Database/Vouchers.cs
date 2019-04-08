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
                return ExecuteQuery(Queries.Vouchers.SelectAll);
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при получении списка путёвок", e);
            }
        }

        public static List<Voucher> Search(string clientFio)
        {
            try
            {
                return ExecuteQuery(Queries.Vouchers.FilterFio(clientFio));
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

        public static List<Voucher> ExecuteQuery(string query)
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
                            string phone = reader.GetString(2);
                            string address = reader.GetString(3);
                            NpgsqlDate birthday = reader.GetDate(4);

                            vouchers.Add(new Voucher(voucherId, fio,
                                birthday, address, phone));
                        }

                        return vouchers;
                    }
                }
            }
        }
    }
}