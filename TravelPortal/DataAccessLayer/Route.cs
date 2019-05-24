using System;
using System.Windows;
using NpgsqlTypes;

namespace TravelPortal.DataAccessLayer
{
    public class Route
    {
        private int _routeId;
        public Visibility CanAddVoucher { get; }

        public string Hotel { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public bool Meels { get; set; }
        public string Transport { get; set; }
        public double FullPrice { get; set; }

        public double HotelPrice { get; set; }
        public double TransportPrice { get; set; }

        public int GetId() => _routeId;

        public Route() { }

        public Route(int routeId, string hotel, string @from, string to, DateTime date, int duration, bool meels, string transport, double fullPrice, double hotelPrice, double transportPrice)
        {
            _routeId = routeId;
            Hotel = hotel;
            From = from;
            To = to;
            Date = date;
            Duration = duration;
            Meels = meels;
            Transport = transport;
            FullPrice = fullPrice;
            HotelPrice = hotelPrice;
            TransportPrice = transportPrice;
            CanAddVoucher = date - DateTime.Now > TimeSpan.FromDays(1)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Hotel): return "Отель";
                case nameof(From): return "Откуда";
                case nameof(To): return "Куда";
                case nameof(Date): return "Дата начала";
                case nameof(Duration): return "Дней";
                case nameof(FullPrice): return "Стоимость, руб.";
                case nameof(Meels): return "Питание";
                case nameof(Transport): return "Транспорт";
                case nameof(HotelPrice): return "Стоимость проживания, руб.";
                case nameof(TransportPrice): return "Стоимость проезда, руб.";
                default: return propertyName;
            }
        }

        public string GetParameterListForFilter()
        {
            string hotel = string.IsNullOrWhiteSpace(Hotel) ? "%" : Hotel;
            string cityFrom = string.IsNullOrWhiteSpace(From) ? "%" : From;
            string cityTo = string.IsNullOrWhiteSpace(To) ? "%" : To;
            string transport = string.IsNullOrWhiteSpace(Transport) ? "%" : Transport;
            return $"'{hotel}', '{cityFrom}', '{cityTo}', '{new NpgsqlDate(Date)}', {Duration}, '{transport}'";
        }
    }
}