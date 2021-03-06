﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Npgsql;
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
                    Route.TransportPrice = _transportPriceDictionary[value];

                }
                else
                {
                    _selectedTransport = null;
                    Route.TransportPrice = 0;
                }

                OnPropertyChanged(nameof(SelectedTransport));
                OnPropertyChanged(nameof(SelectedTransportPrice));
                OnPropertyChanged(nameof(SelectedFullPrice));
            }
        }

        public double SelectedHotelPrice
        {
            get => Route.HotelPrice;
            set
            {
                Route.HotelPrice = value > 0 ? value : Math.Abs(value);
                OnPropertyChanged(nameof(SelectedHotelPrice));
                OnPropertyChanged(nameof(SelectedFullPrice));
            }
        }

        public string SelectedTransportPrice =>
            Route.TransportPrice.ToString("N0") + " руб.";

        public string SelectedFullPrice =>
            Route.FullPrice.ToString("N0") + " руб.";

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

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand UpdateCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public string CaptionText { get; }
        public Visibility CanAddRoute { get; }
        public Visibility CanUpdateRoute { get; }
        public Visibility CancelOnly { get; }
        public bool ReadOnly { get; }

        private readonly Window _window;

        public RouteRecordViewModel(Route route, Window window)
        {
            _window = window;
            if (Route.Empty.Equals(route))
            {
                CaptionText = "Добавление маршрута";
                CanAddRoute = Visibility.Visible;
                CanUpdateRoute = Visibility.Collapsed;
                CancelOnly = Visibility.Collapsed;
                ReadOnly = false;
            }
            else
            {
                CaptionText = "Просмотр маршрута";
                CanAddRoute = Visibility.Collapsed;
                if (route.CanAddVoucher == Visibility.Visible)
                {
                    CanUpdateRoute = Visibility.Visible;
                    CancelOnly = Visibility.Collapsed;
                    ReadOnly = false;
                }
                else
                {
                    CanUpdateRoute = Visibility.Collapsed;
                    CancelOnly = Visibility.Visible;
                    ReadOnly = true;
                }
            }

            _sourceRoute = route;
            Route = new Route(_sourceRoute);
            _hotelCityTypeDictionary = new Dictionary<string, KeyValuePair<string, int>>();
            _transportPriceDictionary = new Dictionary<string, double>();
            HotelCollection = new List<string>();
            CityFromCollection = new List<string>();
            CityToCollection = new List<string>();
        }

        public void Loaded()
        {
            _hotelCityTypeDictionary =
                Dictionaries.GetHotelCityTypeCollection();
            if (CanUpdateRoute == Visibility.Visible || ReadOnly)
            {
                if (_hotelCityTypeDictionary.ContainsKey(Route.Name))
                {
                    KeyValuePair<string, int> hotelInfo =
                        _hotelCityTypeDictionary[Route.Name];
                    _hotelCityTypeDictionary.Clear();
                    _hotelCityTypeDictionary.Add(Route.Name, hotelInfo);
                    HotelCollection = _hotelCityTypeDictionary.Keys.ToList();
                    CityFromCollection = new List<string> {Route.From};
                }
            }
            else
            {
                // Получаем две статические коллекции.
                HotelCollection = _hotelCityTypeDictionary.Keys.ToList();
                CityFromCollection =
                    Dictionaries.GetNameList(DictionaryKind.City);
            }

            // Устанавливаем выбранные элементы.
            SelectedHotel = Route.Name; // В свойcтве также получаем и устанавливаем
                                        // CityToCollection и SelectedCityTo.
            OnPropertyChanged(nameof(SelectedHotelPrice));
            SelectedCityFrom = Route.From;
            SelectedTransport = Route.Transport;

            AddCommand = new RelayCommand(o =>
                {
                    try
                    {
                        MainTables.ExecuteAddUpdateQuery(
                            Queries.MainTables.InsertRoute(Route));
                        _window.Hide();
                    }
                    catch (Exception ex)
                    {
                        OnMessageBoxDisplayRequest("Ошибка добавления маршрута",
                            ex is PostgresException pex
                                ? pex.MessageText
                                : ex.Message);
                    }
                },
                o =>
                {
                    Route.Name = SelectedHotel;
                    Route.From = SelectedCityFrom;
                    Route.To = SelectedCityTo;
                    Route.Transport = SelectedTransport;
                    Route.HotelPrice = SelectedHotelPrice;
                    return Route.IsReadyToInsert();
                });
            OnPropertyChanged(nameof(AddCommand));

            UpdateCommand = new RelayCommand(o =>
            {
                try
                {
                    MainTables.ExecuteAddUpdateQuery(
                        Queries.MainTables.UpdateRoute(Route));
                    _window.Hide();
                }
                catch (Exception ex)
                {
                    OnMessageBoxDisplayRequest("Ошибка изменения маршрута",
                        ex is PostgresException pex
                            ? pex.MessageText
                            : ex.Message);
                }
            }, o =>
            {
                Route.Transport = SelectedTransport;
                return Route.IsReadyToInsert() &&
                       !Route.Equals(_sourceRoute);
            });
            OnPropertyChanged(nameof(UpdateCommand));

            DeleteCommand = new RelayCommand(o =>
            {
                try
                {
                    MainTables.Execute(Queries.MainTables.DeleteRoute(Route.GetId()));
                    _window.Hide();
                }
                catch (Exception ex)
                {
                    OnMessageBoxDisplayRequest("Ошибка удаления маршрута",
                        ex is PostgresException pex
                            ? pex.MessageText
                            : ex.Message);
                }
            });
            OnPropertyChanged(nameof(DeleteCommand));
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
                //if (_transportPriceDictionary == null ||
                //    _transportPriceDictionary.Count == 0)
                //    throw new Exception($"К сожалению, из г. {_selectedCityFrom}" +
                //                        $" в г. {_selectedCityTo}, где находится выбранный отель, " +
                //                        "нет доступных билетов ни на один транспорт. " +
                //                        "Выберите другой город отправления, другой отель " +
                //                        "либо обратитесь к администратору для добавления билета.");
                OnPropertyChanged(nameof(TransportCollection));
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest(
                    "Ошибка получения билетов на любой из видов транспорта",
                    ex is PostgresException pex
                        ? pex.MessageText
                        : ex.Message);
            }
        }
    }
}
