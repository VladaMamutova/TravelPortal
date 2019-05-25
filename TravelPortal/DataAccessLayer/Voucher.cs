using System;
using System.Windows;

namespace TravelPortal.DataAccessLayer
{
    public class Voucher
    {
        private readonly int _voucherId;
        private int _routeId;
        public Visibility CanCancelVoucher { get; }

        public string Hotel { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public double FullPrice { get; set; }
        public string CustomerFio { get; set; }
        public string Phone { get; set; }

        public int GetId() => _voucherId;

        public Voucher(int voucherId, int routeId, string hotel, DateTime date, int duration,
            double fullPrice, string customerFio, string phone)
        {
            _voucherId = voucherId;
            _routeId = routeId;
            Hotel = hotel;
            Date = date;
            Duration = duration;
            FullPrice = fullPrice;
            CustomerFio = customerFio;
            Phone = phone;
            CanCancelVoucher = date >= DateTime.Today
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Hotel): return "Отель";
                case nameof(Date): return "Дата начала";
                case nameof(Duration): return "Длительность";
                case nameof(FullPrice): return "Полная стоимость";
                case nameof(CustomerFio): return "ФИО клиента";
                case nameof(Phone): return "Телефон";
                default: return propertyName;
            }
        }
    }
}
