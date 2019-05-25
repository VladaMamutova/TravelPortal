using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для страницы маршрутов.
    /// </summary>
    class RoutesViewModel : ViewModelBase
    {
        public List<string> HotelCollection { get; }
        public List<string> CityCollection { get; }
        public List<string> TransportCollection { get; }

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
                              (_filterCommand = new RelayCommand(obj =>
                              {
                                  DateTime date;
                                  try
                                  {
                                      date = Convert.ToDateTime(SelectedDate,
                                          new CultureInfo("en-US"));
                                  }catch { date = new DateTime(1, 1, 1); }
                                  
                                  if (!int.TryParse(SelectedDuration, out int duration))
                                      duration = 0;
                                  try
                                  {
                                      Route route = new Route
                                      {
                                          Hotel = SelectedHotel,
                                          From = SelectedCityFrom,
                                          To = SelectedCityTo,
                                          Date = date,
                                          Duration = duration,
                                          Transport = SelectedTransport
                                      };

                                      Collection =
                                          MainTables.GetRoutes(
                                              Queries.MainTables
                                                  .FilterRoutes(route));
                                  }
                                  catch (Exception ex)
                                  {
                                      OnMessageBoxDisplayRequest("Ошибка при фильтрации записей", ex.Message);
                                  }
                              }));
                return _filterCommand;
            }
        }

        public RoutesViewModel()
        {
            try
            {
                HotelCollection =
                    Dictionaries.GetNameList(DictionaryKind.Hotel);
                TransportCollection =
                    Dictionaries.GetNameList(DictionaryKind.Transport);
                CityCollection = Dictionaries.GetNameList(DictionaryKind.City);
                Collection =
                    MainTables.GetRoutes(Queries.MainTables.GetRoutes());
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest("Ошибка при фильтрации записей", ex.Message);
            }
        }
    }
}
