using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для страницы маршрутов.
    /// </summary>
    class RouteViewModel : INotifyPropertyChanged
    {
        public List<string> HotelCollection { get; }
        public string SelectedHotel { get; set; }

        public List<string> TransportCollection { get; }
        public string SelectedTransport { get; set; }

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

        public int Count => Collection?.Count ?? 0;
        private Window _owner;

        //public RelayCommand AddVoucher => new RelayCommand(
        //    e => ShowDialog(_selectedItem), o => true);

        //private void ShowDialog(object o)
        //{
        //    var view = new AddVoucherWindow(SelectedItem) { Owner = _owner };
        //    view.ShowDialog();
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        public RouteViewModel(Window owner)
        {
            _owner = owner;
            HotelCollection =
                Dictionaries.GetNameView(Queries.SelectHotelNameView);
            TransportCollection =
                Dictionaries.GetNameView(Queries.SelectTransportNameView);
            Collection = Routes.GetAll();
        }
    }
}
