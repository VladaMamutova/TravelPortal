using System;
using System.Windows;
using NpgsqlTypes;

namespace TravelPortal.DataAccessLayer
{
    public class Route : SimpleRecord
    {
        private string _from;
        private string _to;
        private DateTime _date;
        private int _duration;
        private bool _meels;
        private string _transport;
        private double _fullPrice;
        private double _hotelPrice;
        private double _transportPrice;

        public Visibility CanAddVoucher { get; }

        public string From
        {
            get => _from;
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
            }
        }

        public string To
        {
            get => _to;
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public int Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        public bool Meels
        {
            get => _meels;
            set
            {
                _meels = value;
                OnPropertyChanged(nameof(Meels));
            }
        }

        public string Transport
        {
            get => _transport;
            set
            {
                _transport = value;
                OnPropertyChanged(nameof(Transport));
            }
        }

        public double FullPrice
        {
            get => _fullPrice;
            set
            {
                _fullPrice = value;
                OnPropertyChanged(nameof(FullPrice));
            }
        }

        public double HotelPrice
        {
            get => _hotelPrice;
            set
            {
                _hotelPrice = value;
                OnPropertyChanged(nameof(HotelPrice));
                FullPrice = _hotelPrice + _transportPrice;
            }
        }

        public double TransportPrice
        {
            get => _transportPrice;
            set
            {
                _transportPrice = value;
                OnPropertyChanged(nameof(TransportPrice));
                FullPrice = _hotelPrice + _transportPrice;
            }
        }

        public new static readonly Route Empty;

        static Route()
        {
            Empty = new Route(-1, "", "", "",
                DateTime.Today, 0, false, "", 0, 0, 0); }

        public Route(Route route) : base(route.Id, route.Name)
        {
            From = route.From;
            To = route.To;
            Date = route.Date;
            Duration = route.Duration;
            Meels = route.Meels;
            Transport = route.Transport;
            FullPrice = route.FullPrice;
            HotelPrice = route.HotelPrice;
            TransportPrice = route.TransportPrice;
            CanAddVoucher = route.CanAddVoucher;
        }

        public Route(int routeId, string hotel, string from, string to,
            DateTime date, int duration, bool meels, string transport,
            double fullPrice, double hotelPrice, double transportPrice) : base(routeId, hotel)
        {
            From = from;
            To = to;
            Date = date;
            Duration = duration;
            Meels = meels;
            Transport = transport;
            FullPrice = fullPrice;
            HotelPrice = hotelPrice;
            TransportPrice = transportPrice;
            CanAddVoucher = date >= DateTime.Today
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return "Отель";
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

        public override bool IsReadyToInsert()
        {
            return base.IsReadyToInsert() &&
                   !string.IsNullOrWhiteSpace(_from) &&
                   !string.IsNullOrWhiteSpace(_to) &&
                   !string.IsNullOrWhiteSpace(_transport) &&
                   _date >= DateTime.Today && _duration > 0 &&
                   _transportPrice > 0 && _hotelPrice > 0;
        }

        public override string GetParameterList()
        {
            return $"'{Name}', '{From}', '{To}', '{Transport}', " +
                   $"'{new NpgsqlDate(Date)}', {Duration}, {HotelPrice}, " +
                   $"{Meels}";
        }

        public override string GetIdentifiedParameterList()
        {
            return $"{Id}, '{Transport}', '{new NpgsqlDate(Date)}', " +
                   $"{Duration}, {HotelPrice}, {Meels}";
        }

        public string GetParameterListForFilter()
        {
            string hotel = string.IsNullOrWhiteSpace(Name) ? "%" : Name;
            string cityFrom = string.IsNullOrWhiteSpace(From) ? "%" : From;
            string cityTo = string.IsNullOrWhiteSpace(To) ? "%" : To;
            string transport = string.IsNullOrWhiteSpace(Transport) ? "%" : Transport;
            return $"'{hotel}', '{cityFrom}', '{cityTo}', " +
                   $"'{new NpgsqlDate(Date)}', {Duration}, '{transport}'";
        }

        public override bool Equals(ITableEntry record)
        {
            if (!base.Equals(record))
                return false;

            if (!(record is Route route)) return false;

            return string.Compare(From , route.From,
                        StringComparison.CurrentCulture) == 0 &&
                    string.Compare(To, route.To,
                        StringComparison.CurrentCulture) == 0 &&
                    Date == route.Date && Duration == route.Duration &&
                    Meels == route.Meels && 
                    string.Compare(Transport, route.Transport,
                        StringComparison.CurrentCulture) == 0 &&
                    Math.Abs(FullPrice - route.FullPrice) < 0.01 &&
                    Math.Abs(TransportPrice - route.TransportPrice) < 0.01 &&
                    Math.Abs(HotelPrice - route.HotelPrice) < 0.01;
        }

        public override string ToString()
        {
            return $"Id={Id}, Hotel=\"{Name}\", From=\"{From}\", " +
                   $"To=\"{To}\", Date=\"{Date}\", Duration={Duration}, " +
                   $"Meels={Meels}, Transport=\"{Transport}\", FullPrice={FullPrice}, " +
                   $"HotelPrice={HotelPrice}, TransportPrice={TransportPrice})";
        }
    }
}