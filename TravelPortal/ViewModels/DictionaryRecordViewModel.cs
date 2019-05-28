using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using Npgsql;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class DictionaryRecordViewModel : ViewModelBase
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
                    o =>
                    {
                        if (dictionary == DictionaryKind.Agency)
                        {
                            Regex regex = new Regex(@"^\+7(9[0-9]{2}|495|499)\d{7}$");
                            if (!regex.IsMatch(((Agency)Record).Phone)){
                                OnMessageBoxDisplayRequest("Ошибка добавления", "Номер должен быть введён в международном формате (+7), " +
                                                                                      "затем должен быть указан префикс региона и оператора " +
                                                                                      "(900-999, для городских - 495 или 499), далее семь цифр.");
                                return;
                            }
                        }
                        Execute(
                        Queries.Dictionaries.Insert(dictionary, Record),
                        window);
                    }, o => Record.IsReadyToInsert());
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
                            o =>
                            {
                                Regex regex = new Regex(@"^\+7(9[0-9]{2}|495|499)\d{7}$");
                                if (!regex.IsMatch(((Agency)Record).Phone))
                                    OnMessageBoxDisplayRequest("Ошибка изменения данных", "Номер должен быть введён в международном формате (+7), " +
                                                        "затем должен быть указан префикс региона и оператора " +
                                                        "(900-999, для городских - 495 или 499), далее семь цифр.");
                                else Execute(Queries.Dictionaries.Update(dictionary,
                                        Record), window);
                            },
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
                OnMessageBoxDisplayRequest("Ошибка изменения данных",
                    e is PostgresException pex
                        ? pex.MessageText
                        : e.Message);
            }
        }
    }
}