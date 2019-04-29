using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Annotations;
using TravelPortal.Database;
using TravelPortal.Models;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    // Для полной поддержки передачи значений данных от объектов источника
    // для целевых объектов, каждый объект в коллекции, который поддерживает
    // свойства связывания должен также реализовывать INotifyPropertyChanged
    // интерфейс. 
    // Необходимо будет реализовать этот интерфейс, если нужно будет
    // гарантировать изменение и перерисовку таблицы после изменения
    // свойства одного элемента коллекции (т.е. изменить таблицу с Agencies
    // на форме после изменения Agencies[0].Name, например. Если Agencies[0]
    // заменить на новый объект (Agencies[0] = new Agency()), интерфейс
    // реализовывать не надо).
    public class DictionaryViewModel : INotifyPropertyChanged
    {
        private SimpleRecord _transportSelectedItem;
        public SimpleRecord TransportSelectedItem {
            get => _transportSelectedItem;
            set
            {
                _transportSelectedItem = value;
                OnPropertyChanged(nameof(TransportSelectedItem));
            }
        }
        #region private ObservableCollection properties for dictionaries

        private ObservableCollection<Agency> _agencies;
        private ObservableCollection<SimpleRecord> _cities;
        private ObservableCollection<Hotel> _hotels;
        private ObservableCollection<SimpleRecord> _ownership;
        private ObservableCollection<SimpleRecord> _statusCollection;
        private ObservableCollection<Ticket> _tickets;
        private ObservableCollection<SimpleRecord> _transportCollection;

        #endregion

        #region public ObservableCollection properties for dictionaries

        public ObservableCollection<Agency> Agencies {
            get => _agencies;
            set
            {
                _agencies = value;
                OnPropertyChanged(nameof(Agencies));
            }
        }

        public ObservableCollection<SimpleRecord> Cities
        {
            get => _cities;
            set
            {
                _cities = value;
                OnPropertyChanged(nameof(Cities));
            }
        }

        public ObservableCollection<Hotel> Hotels
        {
            get => _hotels;
            set
            {
                _hotels = value;
                OnPropertyChanged(nameof(Hotels));
            }
        }

        public ObservableCollection<SimpleRecord> Ownership
        {
            get => _ownership;
            set
            {
                _ownership = value;
                OnPropertyChanged(nameof(Ownership));
            }
        }

        public ObservableCollection<SimpleRecord> Status
        {
            get => _statusCollection;
            set
            {
                _statusCollection = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ObservableCollection<Ticket> Tickets
        {
            get => _tickets;
            set
            {
                _tickets = value;
                OnPropertyChanged(nameof(Tickets));
            }
        }

        public ObservableCollection<SimpleRecord> TransportCollection
        {
            get => _transportCollection;
            set
            {
                _transportCollection = value;
                OnPropertyChanged(nameof(TransportCollection));
            }
        }

        #endregion

        public DictionaryViewModel()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();

                //Agencies = GetAgencies(connection);
                Cities = GetCollection(connection,
                    Queries.Dictionaries.SelectAllCities);
                //Hotels = GetHotels(connection);
                //Ownership = GetCollection(connection,
                //    Queries.Dictionaries.SelectAllOwnership);
                //Status = GetCollection(connection,
                //    Queries.Dictionaries.SelectAllStatus);
                //Tickets = GetTickets(connection);
                TransportCollection = GetCollection(connection,
                    Queries.Dictionaries.SelectAllTransport);
                //if (TransportCollection.Count > 0)TransportSelectedItem = TransportCollection[0];
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        #region Methods for getting data from dictionaries to ObservableCollections

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

        private static ObservableCollection<SimpleRecord> GetCollection(NpgsqlConnection npgsqlConnection, string query)
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

        #region Commands

        public RelayCommand AddTransportCommand => new RelayCommand(e => ExecuteRunDialog(null));
        public RelayCommand ModifyTransportCommand => new RelayCommand(e => ExecuteRunDialog(_transportSelectedItem), o => _transportSelectedItem != null);
        
        private void ExecuteRunDialog(object o)
        {
            var view = new DictionaryRecordDialog();
            SimpleRecord simpleRecordCopy = new SimpleRecord((SimpleRecord)o);
            DictionaryRecordViewModel viewModel =
                new DictionaryRecordViewModel(DictionaryModels.Transport, view, simpleRecordCopy);
            view.DataContext = viewModel;

            view.ShowDialog();
            NpgsqlConnection c = new NpgsqlConnection(Configuration.GetConnetionString());
            c.Open();
            TransportCollection = GetCollection(c,Queries.Dictionaries.SelectAllTransport);
            TransportSelectedItem =
                TransportCollection.SingleOrDefault(i => i.GetId() == simpleRecordCopy.GetId());
        }
        
        #endregion
    }
}