using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для страницы маршрутов.
    /// </summary>
    class RoutesViewModel : ViewModelBase
    {
        private string _selectedHotel;
        public string SelectedHotel
        {
            get => _selectedHotel;
            set
            {
                _selectedHotel = value;
                OnPropertyChanged(nameof(SelectedHotel));
            }
        }

        private string _selectedCityFrom;
        public string SelectedCityFrom
        {
            get => _selectedCityFrom;
            set
            {
                _selectedCityFrom = value;
                OnPropertyChanged(nameof(SelectedCityFrom));
            }
        }

        private string _selectedCityTo;
        public string SelectedCityTo
        {
            get => _selectedCityTo;
            set
            {
                _selectedCityTo = value;
                OnPropertyChanged(nameof(SelectedCityTo));
            }
        }

        private string _selectedDate;
        public string SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private string _selectedDuration;
        public string SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                _selectedDuration = value;
                OnPropertyChanged(nameof(SelectedDuration));
            }
        }

        private string _selectedTransport;
        public string SelectedTransport
        {
            get => _selectedTransport;
            set
            {
                _selectedTransport = value;
                OnPropertyChanged(nameof(SelectedTransport));
            }
        }

        private Route _selectedItem;
        public Route SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private List<string> _hotelCollection;
        public List<string> HotelCollection
        {
            get => _hotelCollection;
            set
            {
                _hotelCollection = value;
                OnPropertyChanged(nameof(HotelCollection));
            }
        }

        private List<string> _cityCollection;
        public List<string> CityCollection
        {
            get => _cityCollection;
            set
            {
                _cityCollection = value;
                OnPropertyChanged(nameof(CityCollection));
            }
        }

        private List<string> _transportCollection;
        public List<string> TransportCollection
        {
            get => _transportCollection;
            set
            {
                _transportCollection = value;
                OnPropertyChanged(nameof(TransportCollection));
            }
        }

        private ObservableCollection<Route> _collection;
        public ObservableCollection<Route> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand
        {
            get
            {
                _filterCommand = _filterCommand ??
                              (_filterCommand = new RelayCommand(FilterRoutes));
                return _filterCommand;
            }
        }

        private RelayCommand _newRouteCommand;
        public RelayCommand NewRouteCommand
        {
            get
            {
                _newRouteCommand = _newRouteCommand ??
                                 (_newRouteCommand = new RelayCommand(o =>
                                 {
                                     OnDialogDisplayRequest(Route.Empty);
                                     FilterCommand.Execute(null);
                                 }));
                return _newRouteCommand;
            }
        }

        public RoutesViewModel()
        {
            HotelCollection = new List<string>();
            TransportCollection = new List<string>();
            CityCollection = new List<string>();
            Collection = new ObservableCollection<Route>();
        }

        public void LoadFromDb()
        {
            try
            {
                HotelCollection =
                    Dictionaries.GetNameList(DictionaryKind.Hotel);
                TransportCollection =
                    Dictionaries.GetNameList(DictionaryKind.Transport);
                CityCollection = Dictionaries.GetNameList(DictionaryKind.City);
                SelectedDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                Collection =
                    MainTables.GetRoutes(
                        Queries.MainTables.FilterRoutes(new Route(Route.Empty)
                            {Date = DateTime.Today}));
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest("Ошибка загрузки данных из базы", ex.Message);
            }
        }

        public void FilterRoutes(object o)
        {
            DateTime date;
            try
            {
                date = Convert.ToDateTime(SelectedDate,
                    new CultureInfo("en-US"));
            }
            catch { date = new DateTime(1, 1, 1); }

            if (!int.TryParse(SelectedDuration, out int duration))
                duration = 0;
            try
            {
                Route route = new Route(Route.Empty)
                {
                    Name = SelectedHotel,
                    From = SelectedCityFrom,
                    To = SelectedCityTo,
                    Date = date,
                    Duration = duration,
                    Transport = SelectedTransport
                };

                Collection = MainTables.GetRoutes(Queries.MainTables.FilterRoutes(route));
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest("Ошибка при получении списка маршрутов", ex.Message);
            }
        }
    }
}
