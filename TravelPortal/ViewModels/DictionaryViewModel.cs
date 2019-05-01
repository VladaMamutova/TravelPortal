using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MaterialDesignThemes.Wpf;
using Npgsql;
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

        public DictionaryViewModel(DictionaryKind dictionary)
        {
            _dictionary = dictionary;
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
                    IconKind = PackIconKind.Hotel;
                    GenerateTitleFunc = Ticket.GenerateTitle;
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
            var view = new DictionaryRecordDialog();
            SimpleRecord recordCopy;
            switch (_dictionary)
            {
                case DictionaryKind.Hotel:
                    recordCopy = new Hotel(o != null
                        ? (Hotel) o
                        : Hotel.Empty);
                    break;
                default:
                    recordCopy = new SimpleRecord(o != null
                        ? (SimpleRecord) o
                        : SimpleRecord.Empty);
                    break;
            }


            DictionaryRecordViewModel viewModel =
                new DictionaryRecordViewModel(_dictionary, view, recordCopy);
            view.DataContext = viewModel;

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
            ObservableCollection<SimpleRecord> collection = Dictionaries.GetDictionary(_dictionary);
            // Вывод сообщений при исключениях.
            switch (_dictionary)
            {
                case DictionaryKind.Hotel:
                    foreach (var item in collection)
                        Collection.Add((Hotel)item);
                    break;
                default:
                    foreach (var item in collection)
                        Collection.Add(item);
                    break;
            }
        }
    }
}