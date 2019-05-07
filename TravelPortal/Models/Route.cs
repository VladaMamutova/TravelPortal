using System;
using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Route
    {
        private int _routeId;
        //private int _residenceId;
        //private int _transportTypeId;
        //private int _agencyId;

        public string Hotel { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public int Duration { get; set; }
        public double Price { get; set; }
        public bool Meels { get; set; }
        public string Transport { get; set; }
        public double TransportPrice { get; set; }

        public Route(int routeId, string hotel, string from, string to, double price, DateTime date, int duration,
            bool meels, string transport, double transportPrice)
        {
            _routeId = routeId;
            Hotel = hotel;
            From = from;
            To = to;
            Price = price;
            Date = date;
            Duration = duration;
            Meels = meels;
            Transport = transport;
            TransportPrice = transportPrice;
        }
    }
}