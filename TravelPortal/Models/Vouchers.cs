using System;
using System.Collections.ObjectModel;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    public class Vouchers
    {
        public static ObservableCollection<Voucher> GetAll()
        {
            try
            {
                return MainTables.GetVouchers(Queries.GetVouchers());
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
                return MainTables.GetVouchers(Queries.Vouchers.Search(clientFio));
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
                return MainTables.GetVouchers(Queries.Vouchers.FilterStatus(status));
            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при получении списка путёвок", e);
            }
        }
    }
}