using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Npgsql;
using TravelPortal.Database;

namespace TravelPortal.ViewModels
{
    class Hotel
    {
        public string Name { get; set; }

        public Hotel(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }

    class RouteViewModel : INotifyPropertyChanged
    {
        //public CollectionView Hotels { get; }

        private readonly CollectionView _hotelCollection;
        private string _hotel;

        public CollectionView HotelCollection => _hotelCollection;

        public string Hotel
        {
            get => _hotel;
            set
            {
                if (_hotel == value) return;
                _hotel = value;
                OnPropertyChanged("Hotel");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public RouteViewModel()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command =
                    new NpgsqlCommand(Queries.SelectAllHotels, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) _hotelCollection = null;
                        List<Hotel> hotels = new List<Hotel>();
                        while (reader.Read())
                        {
                            hotels.Add(new Hotel(reader.GetString(0).TrimEnd(' ')));
                        }

                        _hotelCollection = new CollectionView(hotels);
                    }
                }
            }
        }
    }
}
