using System;
using System.Collections.ObjectModel;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public class Vouchers
    {
        public static ObservableCollection<Voucher> GetVouchers()
        {
            try
            {
                return ExecuteQuery(Queries.SelectVoucherView);
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при получении списка путёвок", e);
            }
        }

        public static ObservableCollection<Voucher> SearchByClientFio(string clientFio)
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

        public static ObservableCollection<Voucher> SearchByStatus(string status)
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

        private static ObservableCollection<Voucher> ExecuteQuery(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
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
    }
}