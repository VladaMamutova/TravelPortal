using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MaterialDesignThemes.Wpf;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Annotations;
using TravelPortal.Database;
using TravelPortal.Models;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    public class DictionaryViewModel : INotifyPropertyChanged
    {
        public string Title { get; }
        public PackIconKind IconKind { get; }

        private readonly DictionaryModels _dictionary;

        public DictionaryViewModel(DictionaryModels dictionary)
        {
            _dictionary = dictionary;
            switch (_dictionary)
            {
                case DictionaryModels.Transport:
                    Title = "Вид транспорта";
                    IconKind = PackIconKind.Aeroplane;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryModels.City:
                    Title = "Города";
                    IconKind = PackIconKind.City;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryModels.Ownership:
                    Title = "Тип собственности";
                    IconKind = PackIconKind.SecurityHome;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryModels.Status:
                    Title = "Социнальное положение";
                    IconKind = PackIconKind.TicketUser;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
            }

            UpdateCollection();
        }

        private SimpleRecord _selectedItem;
        public SimpleRecord SelectedItem {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<SimpleRecord> _collection;
        public ObservableCollection<SimpleRecord> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        public int Count => Collection.Count;

        #region Commands

        public RelayCommand AddCommand =>
            new RelayCommand(e => ShowDialog(null));

        public RelayCommand ModifyCommand => new RelayCommand(
            e => ShowDialog(_selectedItem), o => SelectedItem != null);

        private void ShowDialog(object o)
        {
            var view = new DictionaryRecordDialog();
            SimpleRecord recordCopy = new SimpleRecord(o != null ? (SimpleRecord) o : SimpleRecord.Empty);
            DictionaryRecordViewModel viewModel =
                new DictionaryRecordViewModel(_dictionary, view, recordCopy);
            view.DataContext = viewModel;

            view.ShowDialog();
            UpdateCollection();
            SelectedItem = Collection.SingleOrDefault(i => 
                    i.GetId() == recordCopy.GetId());
        }

        #endregion

        public Func<string, string> GenerateTitleFunc { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        #region Methods for getting data from dictionaries to ObservableCollections

        private void UpdateCollection()
        {
            string query = "";
            switch (_dictionary)
            {
                case DictionaryModels.Transport:
                    query = Queries.Dictionaries.SelectAllTransport;
                    break;
                case DictionaryModels.City:
                    query = Queries.Dictionaries.SelectAllCities;
                    break;
                case DictionaryModels.Ownership:
                    query = Queries.Dictionaries.SelectAllOwnership;
                    break;
                case DictionaryModels.Status:
                    query = Queries.Dictionaries.SelectAllStatus;
                    break;
            }

            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                Collection = GetSimpleDictionary(connection, query);
            }
        }

        private static ObservableCollection<Agency> GetAgencies(NpgsqlConnection npgsqlConnection)
        {
            using (var command = new NpgsqlCommand(
                Queries.Dictionaries.SelectAllAgencies, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<Agency> collection = new ObservableCollection<Agency>();
                    while (reader.Read())
                    {
                        int agencyId = reader.GetInt32(0);
                        string registration = reader.GetString(1).TrimEnd();
                        string name = reader.GetString(2).TrimEnd();
                        string city = reader.GetString(3).TrimEnd();
                        string address = reader.GetString(4).TrimEnd();
                        string ownership = reader.GetString(5).TrimEnd();
                        string phone = reader.GetString(6).TrimEnd();
                        NpgsqlDate date = reader.GetDate(7);

                        collection.Add(new Agency(agencyId, registration, name,
                            city, address, ownership, phone, date));
                    }

                    return collection;
                }
            }
        }

        private static ObservableCollection<Hotel> GetHotels(NpgsqlConnection npgsqlConnection)
        {
            using (var command = new NpgsqlCommand(
                Queries.Dictionaries.SelectAllHotels, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<Hotel> collection = new ObservableCollection<Hotel>();
                    while (reader.Read())
                    {
                        int hotelId = reader.GetInt32(0);
                        string name = reader.GetString(1).TrimEnd();
                        int type = reader.GetInt32(2);
                       
                        collection.Add(new Hotel(hotelId, name, type));
                    }

                    return collection;
                }
            }
        }

        private static ObservableCollection<Ticket> GetTickets(NpgsqlConnection npgsqlConnection)
        {
            using (var command = new NpgsqlCommand(
                Queries.Dictionaries.SelectAllTickets, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<Ticket> collection = new ObservableCollection<Ticket>();
                    while (reader.Read())
                    {
                        int ticketId = reader.GetInt32(0);
                        string from = reader.GetString(1).TrimEnd();
                        string to = reader.GetString(2).TrimEnd();
                        string transport = reader.GetString(3).TrimEnd();
                        double cost = reader.GetDataTypeOID(4);

                        collection.Add(new Ticket(ticketId, from, to, transport, cost));
                    }

                    return collection;
                }
            }
        }

        private static ObservableCollection<SimpleRecord> GetSimpleDictionary(NpgsqlConnection npgsqlConnection, string query)
        {
            using (var command = new NpgsqlCommand(query, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<SimpleRecord> collection = new ObservableCollection<SimpleRecord>();
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1).TrimEnd();

                        collection.Add(new SimpleRecord(id, name));
                    }

                    return collection;
                }
            }
        }

        #endregion
    }
}