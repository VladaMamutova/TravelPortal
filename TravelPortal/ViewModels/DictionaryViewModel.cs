﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using MaterialDesignThemes.Wpf;
using Npgsql;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class DictionaryViewModel : ViewModelBase
    {
        public string Title { get; }
        public PackIconKind IconKind { get; }
        public DictionaryKind Dictionary { get; }

        
        public DictionaryViewModel(DictionaryKind dictionary)
        {
            Dictionary = dictionary;
            switch (Dictionary)
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
                    Title = "Проездные билеты";
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

        public RelayCommand AddCommand =>
            new RelayCommand(e => ShowDialog(null));

        public RelayCommand UpdateCommand => new RelayCommand(
            e => ShowDialog(_selectedItem), o => SelectedItem != null);

        public RelayCommand DeleteCommand => new RelayCommand(
            e =>
            {
                try
                {
                    Dictionaries.ExecuteQuery(
                        Queries.Dictionaries.Delete(Dictionary,
                            (SimpleRecord) _selectedItem));
                }
                catch (Exception ex)
                {
                   OnMessageBoxDisplayRequest("Ошибка удаления", ex is PostgresException pex
                       ? pex.MessageText
                       : ex.Message);
                }

                UpdateCollection();
            }, o => SelectedItem != null);

        private void ShowDialog(object o)
        {
            SimpleRecord recordCopy;
            switch (Dictionary)
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

            OnDialogDisplayRequest(recordCopy);
            
            UpdateCollection();
            SelectedItem = Collection.SingleOrDefault(i => 
                    ((SimpleRecord)i).GetId() == recordCopy.GetId());
        }

        public Func<string, string> GenerateTitleFunc { get; }

        private void UpdateCollection()
        {
            Collection = new ObservableCollection<object>();
            ObservableCollection<SimpleRecord> collection;
            try
            {
                collection = Dictionaries.GetDictionary(Dictionary);
            }
            catch (Exception e)
            {
                OnMessageBoxDisplayRequest("Ошибка получения данных",
                    e is PostgresException pex
                        ? pex.MessageText
                        : e.Message);
                return;
            }
          
            switch (Dictionary)
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