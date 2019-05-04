using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using Npgsql;
using TravelPortal.Annotations;
using TravelPortal.Database;
using TravelPortal.Models;

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

        private void Execute(object query, Window window)
        {
            try
            {
                using (var connection =
                    new NpgsqlConnection(Configuration.GetConnetionString()))
                {
                    connection.Open();
                    using (var command =
                        new NpgsqlCommand((string) query, connection))
                    {
                        object result =
                            command
                                .ExecuteScalar();
                        _sourceRecord.Name = Record.Name;
                        if (int.TryParse(result?.ToString(), out var id))
                            _sourceRecord.SetId(id);
                        window.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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