using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    public class DictionaryRecordViewModel :INotifyPropertyChanged
    {
        private SimpleRecord _record;
        private readonly SimpleRecord _sourceRecord;
        public SimpleRecord Record
        {
            get => _record;
            set
            {
                _record = value;
                OnPropertyChanged(nameof(Record));
            }
        }

        public string Title { get; }
        public string CommandText { get; }
        public RelayCommand Command { get; }

        public DictionaryRecordViewModel(DictionaryKind dictionary,
            Window window, [NotNull] SimpleRecord record)
        {

            CommandText = SimpleRecord.Empty.Equals(record)
                ? "ДОБАВИТЬ"
                : "ИЗМЕНИТЬ";
            _sourceRecord = record;
            switch (dictionary)
            {
                case DictionaryKind.Transport:
                {
                    Title = "Вид транспорта";
                    Record = new SimpleRecord(record);
                    break;
                }
                case DictionaryKind.City:
                {
                    Title = "Город";
                    Record = new SimpleRecord(record);
                    break;
                }
                case DictionaryKind.Ownership:
                {
                    Title = "Тип собственности";
                    Record = new SimpleRecord(record);
                    break;
                }
                case DictionaryKind.Status:
                {
                    Title = "Социальное положение";
                    Record = new SimpleRecord(record);
                    break;
                }
                case DictionaryKind.Hotel:
                {
                    Title = "Отель";
                    Record = new Hotel((Hotel) record);
                    break;
                }
                case DictionaryKind.Ticket:
                {
                    Title = "Билет";
                    Record = new Ticket((Ticket) record);
                    break;
                }
                case DictionaryKind.Agency:
                {
                    Title = "Агенство";
                    Record = new Agency((Agency) record);
                    break;
                }
                default:
                    throw new InvalidEnumArgumentException(nameof(dictionary));
            }

            if (SimpleRecord.Empty.Equals(record))
                Command = new RelayCommand(
                    o => Execute(
                        Queries.Dictionaries.Insert(dictionary, Record),
                        window), o => Record.IsReadyToInsert());
            else
            {
                switch (dictionary)
                {
                    case DictionaryKind.Hotel:
                        Command = new RelayCommand(
                            o => Execute(
                                Queries.Dictionaries.Update(dictionary,
                                    Record), window),
                            o => Record.IsReadyToInsert() &&
                                 !((Hotel) Record).Equals(
                                     (Hotel) _sourceRecord));
                        break;
                    case DictionaryKind.Ticket:
                        Command = new RelayCommand(
                            o => Execute(
                                Queries.Dictionaries.Update(dictionary,
                                    Record), window),
                            o => Record.IsReadyToInsert() &&
                                 !((Ticket)Record).Equals(
                                     (Ticket)_sourceRecord));
                        break;
                    case DictionaryKind.Agency:
                        Command = new RelayCommand(
                            o => Execute(
                                Queries.Dictionaries.Update(dictionary,
                                    Record), window),
                            o => Record.IsReadyToInsert() &&
                                 !((Agency)Record).Equals(
                                     (Agency)_sourceRecord));
                        break;
                    default:
                        Command = new RelayCommand(
                            o => Execute(
                                Queries.Dictionaries.Update(dictionary,
                                    Record), window),
                            o => Record.IsReadyToInsert() &&
                                 Record.Equals(_sourceRecord));
                        break;
                }
            }
        }

        private void Execute(string query, Window window)
        {
            try
            {
                object result = Dictionaries.ExecuteQuery(query);
                _sourceRecord.Name = Record.Name;
                if (int.TryParse(result?.ToString(), out var id))
                    _sourceRecord.SetId(id);
                window.Close();
            }
            catch (Exception e)
            {
                CustomMessageBox.Show("Ошибка при выполнении запроса", e.Message);
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
    }
}