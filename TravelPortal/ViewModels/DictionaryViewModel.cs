using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MaterialDesignThemes.Wpf;
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

        private readonly DictionaryKind _dictionary;
        private Window _owner;

        public DictionaryViewModel(DictionaryKind dictionary, Window owner)
        {
            _dictionary = dictionary;
            _owner = owner;
            switch (_dictionary)
            {
                case DictionaryKind.Transport:
                    Title = "Вид транспорта";
                    IconKind = PackIconKind.Aeroplane;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryKind.City:
                    Title = "Города";
                    IconKind = PackIconKind.City;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryKind.Ownership:
                    Title = "Тип собственности";
                    IconKind = PackIconKind.SecurityHome;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryKind.Status:
                    Title = "Социальное положение";
                    IconKind = PackIconKind.TicketUser;
                    GenerateTitleFunc = SimpleRecord.GenerateTitle;
                    break;
                case DictionaryKind.Hotel:
                    Title = "Отели";
                    IconKind = PackIconKind.Hotel;
                    GenerateTitleFunc = Hotel.GenerateTitle;
                    break;
                case DictionaryKind.Ticket:
                    Title = "Билеты на проезд";
                    IconKind = PackIconKind.Cards;
                    GenerateTitleFunc = Ticket.GenerateTitle;
                    break;
                case DictionaryKind.Agency:
                    Title = "Агенства";
                    IconKind = PackIconKind.OfficeBuilding;
                    GenerateTitleFunc = Agency.GenerateTitle;
                    break;
            }

            UpdateCollection();
        }

        private object _selectedItem;
        public object SelectedItem {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<object> _collection;
        public ObservableCollection<object> Collection
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
            SimpleRecord recordCopy;
            switch (_dictionary)
            {
                case DictionaryKind.Hotel:
                    recordCopy = new Hotel(o != null ? (Hotel) o : Hotel.Empty);
                    break;
                case DictionaryKind.Ticket:
                    recordCopy =
                        new Ticket(o != null ? (Ticket) o : Ticket.Empty);
                    break;
                case DictionaryKind.Agency:
                    recordCopy =
                        new Agency(o != null ? (Agency) o : Agency.Empty);
                    break;
                default:
                    recordCopy = new SimpleRecord(o != null
                        ? (SimpleRecord) o
                        : SimpleRecord.Empty);
                    break;
            }

            var view = new DictionaryRecordDialog(_dictionary, recordCopy) {Owner = _owner};
            view.ShowDialog();

            UpdateCollection();
            SelectedItem = Collection.SingleOrDefault(i => 
                    ((SimpleRecord)i).GetId() == recordCopy.GetId());
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

        private void UpdateCollection()
        {
            Collection = new ObservableCollection<object>();
            ObservableCollection<SimpleRecord> collection;
            try
            {
                collection = Dictionaries.GetDictionary(_dictionary);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Ошибка получения данных");
                return;
            }
          
            switch (_dictionary)
            {
                case DictionaryKind.Hotel:
                    foreach (var item in collection)
                        Collection.Add((Hotel)item);
                    break;
                case DictionaryKind.Ticket:
                    foreach (var item in collection)
                        Collection.Add((Ticket)item);
                    break;
                case DictionaryKind.Agency:
                    foreach (var item in collection)
                        Collection.Add((Agency)item);
                    break;
                default:
                    foreach (var item in collection)
                        Collection.Add(item);
                    break;
            }
        }
    }
}