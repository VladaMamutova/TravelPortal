using System;
using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Voucher
    {
        private int _voucherId;

        public string Hotel { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public double FullPrice { get; set; }
        public string CustomerFio { get; set; }
        public string Phone { get; set; }

        public Voucher(int voucherId, string hotel, DateTime date, int duration,
            double fullPrice, string customerFio, string phone)
        {
            _voucherId = voucherId;
            Hotel = hotel;
            Date = date;
            Duration = duration;
            FullPrice = fullPrice;
            CustomerFio = customerFio;
            Phone = phone;
        }
    }
}
