using System;
using System.Collections.Generic;
using System.Linq;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    class RouteRecordViewModel: ViewModelBase
    {
        Dictionary<string, KeyValuePair<string, int>> _hotelCityTypeDictionary;
        Dictionary<string, double> _transportPriceDictionary;

        private string _selectedHotel;
        private int _selectedHotelType;
        private string _selectedCityFrom;
        private string _selectedCityTo;
        private string _selectedTransport;
        private double _selectedTransportPrice;
        private double _selectedHotelPrice;
        private double _selectedFullPrice;

        private List<string> _hotelCollection;
        private List<string> _cityFromCollection;
        private List<string> _cityToCollection;
        private List<string> _transportCollection;

        private readonly Route _sourceRoute;
        private Route _route;
        
        public Route Route
        {
            get => _route;
            set
            {
                _route = value;
                OnPropertyChanged(nameof(Route));
            }
        }

        public string CommandText { get; }
        public string CaptionText { get; }

        public string SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                if (_hotelCityTypeDictionary != null && value != null &&
                    _hotelCityTypeDictionary.ContainsKey(value))
                {
                    _selectedHotel = value;
                    SelectedCityTo = _hotelCityTypeDictionary[value].Key;
                    SelectedHotelType = _hotelCityTypeDictionary[value].Value;
                    CityToCollection = new List<string>
                        {_hotelCityTypeDictionary[SelectedHotel].Key};
                }
                else
                {
                    _selectedHotel = null;
                    SelectedCityTo = null;
                    SelectedHotelType = 0;
                }
                Route.Name = _selectedHotel;
                OnPropertyChanged(nameof(SelectedHotel));
            }
        }

        public int SelectedHotelType
        {
            get => _selectedHotelType;
            set
            {
                _selectedHotelType = value;
                OnPropertyChanged(nameof(SelectedHotelType));
            }
        }

        public string SelectedCityFrom
        {
            get => _selectedCityFrom;
            set
            {
                _selectedCityFrom = value;
                OnPropertyChanged(nameof(SelectedCityFrom));
                TryUpdateTransportCollection();
            }
        }

        public string SelectedCityTo
        {
            get => _selectedCityTo;
            set
            {
                _selectedCityTo = value;
                OnPropertyChanged(nameof(SelectedCityTo));
                TryUpdateTransportCollection();
            }
        }

        public string SelectedTransport
        {
            get => _selectedTransport;
            set
            {
                if (_transportPriceDictionary != null && value != null &&
                    _transportPriceDictionary.ContainsKey(value))
                {
                    _selectedTransport = value;
                    _selectedTransportPrice = _transportPriceDictionary[value];
                    _selectedFullPrice =
                        _selectedTransportPrice + _selectedHotelPrice;

                }
                else
                {
                    _selectedTransport = null;
                    _selectedTransportPrice = 0;
                }

                OnPropertyChanged(nameof(SelectedTransport));
                OnPropertyChanged(nameof(SelectedTransportPrice));
                OnPropertyChanged(nameof(SelectedFullPrice));
            }
        }

        public double SelectedHotelPrice
        {
            get => _selectedHotelPrice;
            set
            {
                _selectedHotelPrice = value > 0 ? value : Math.Abs(value);
                _selectedFullPrice =
                    _selectedTransportPrice + _selectedHotelPrice;
                OnPropertyChanged(nameof(SelectedHotelPrice));
                OnPropertyChanged(nameof(SelectedFullPrice));
            }
        }

        public string SelectedTransportPrice =>
            _selectedTransportPrice.ToString("N0") + " руб.";

        public string SelectedFullPrice =>
            _selectedFullPrice.ToString("N0") + " руб.";

        public List<string> HotelCollection
        {
            get => _hotelCollection;
            set
            {
                _hotelCollection = value;
                OnPropertyChanged(nameof(HotelCollection));
            }
        }

        public List<string> CityFromCollection
        {
            get => _cityFromCollection;
            set
            {
                _cityFromCollection = value;
                OnPropertyChanged(nameof(CityFromCollection));
            }
        }

        public List<string> CityToCollection
        {
            get => _cityToCollection;
            set
            {
                _cityToCollection = value;
                OnPropertyChanged(nameof(CityToCollection));
            }
        }

        public List<string> TransportCollection
        {
            get => _transportCollection;
            set
            {
                _transportCollection = value;
                OnPropertyChanged(nameof(TransportCollection));
            }
        }

        public RouteRecordViewModel(Route route)
        {
            CommandText = Route.Empty.Equals(route)
                ? "ДОБАВИТЬ"
                : "ИЗМЕНИТЬ";
            CaptionText = Route.Empty.Equals(route)
                ? "Добавление маршрута"
                : "Изменение маршрута";
            _sourceRoute = route;
            _hotelCityTypeDictionary = new Dictionary<string, KeyValuePair<string, int>>();
        }


        public void Loaded()
        {
            Route = new Route(_sourceRoute);
            _hotelCityTypeDictionary =
                Dictionaries.GetHotelCityTypeCollection();
            // Получаем две статические коллекции.
            HotelCollection = _hotelCityTypeDictionary.Keys.ToList();
            CityFromCollection = Dictionaries.GetNameList(DictionaryKind.City);

            // Устанавливаем выбранные элементы.
            SelectedHotel = Route.Name; // В свойcтве также получаем и устанавливаем
                                        // CityToCollection и SelectedCityTo.
            _selectedHotelPrice = Route.HotelPrice;
            OnPropertyChanged(nameof(SelectedHotelPrice));
            SelectedCityFrom = Route.From;
            SelectedTransport = Route.Transport;
        }

        public void TryUpdateTransportCollection()
        {
            if (_selectedCityFrom == null || _selectedCityTo == null) return;
            try
            {
                _transportPriceDictionary = Dictionaries.GetTransportPriceCollection(
                    _selectedCityFrom,
                    _selectedCityTo);
                TransportCollection = _transportPriceDictionary?.Keys.ToList();
                if (_transportPriceDictionary == null ||
                    _transportPriceDictionary.Count == 0)
                    throw new Exception($"К сожалению, из г. {_selectedCityFrom}" +
                                        $" в г. {_selectedCityTo}, где находится выбранный отель, " +
                                        "нет доступных билетов ни на один транспорт. " +
                                        "Выберите другой город отправления, другой отель " +
                                        "либо обратитесь к администратору для добавления билета.");
                OnPropertyChanged(nameof(TransportCollection));
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest(
                    "Ошибка получения билетов на любой из видов транспорта",
                    ex.Message);
            }
        }
    }
}
