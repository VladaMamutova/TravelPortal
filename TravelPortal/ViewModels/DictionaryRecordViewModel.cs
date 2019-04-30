using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
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
        private object _record;
        private readonly object _sourceRecord;
        public object Record
        {
            get => _record;
            set
            {
                _record = value;
                OnPropertyChanged(nameof(Record));
            }
        }

        public string Title { get; }
        public TextBox[] InputBoxes { get; }
        public string CommandText { get; }
        public RelayCommand Command { get; }

        public DictionaryRecordViewModel(DictionaryModels dictionary, Window window, [NotNull]object record)
        {
            
            CommandText = ((SimpleRecord)record).Equals(SimpleRecord.Empty) ? "ДОБАВИТЬ" : "ИЗМЕНИТЬ";
            Record = new SimpleRecord((SimpleRecord)record);
            _sourceRecord = record;
            switch (dictionary)
            {
                case DictionaryModels.Transport:
                {
                    Title = "Вид транспорта";
                    InputBoxes = GenerateInputBoxes();
                    if (((SimpleRecord)record).Equals(SimpleRecord.Empty))
                        Command = new RelayCommand(
                            o => Add(
                                Queries.Dictionaries.Insert(
                                    ((SimpleRecord)Record).Name), window),
                            o => !string.IsNullOrEmpty(((SimpleRecord) Record).Name));
                    else
                    {
                        Command = new RelayCommand(
                            o => Add(
                                Queries.Dictionaries.Update(((SimpleRecord)Record).GetId(),
                                    ((SimpleRecord)Record).Name), window),
                            o => !string.IsNullOrEmpty(((SimpleRecord)Record)
                                .Name) && !((SimpleRecord)Record).Equals((SimpleRecord)_sourceRecord));
                        }
                    break;
                }
                default: throw new InvalidEnumArgumentException(nameof(dictionary));


            }
        }
        
        private TextBox[] GenerateInputBoxes()
        {
            TextBox textBox = new TextBox();
            textBox.SetBinding(TextBox.TextProperty,
                new Binding("Record.Name") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Mode = BindingMode.TwoWay });
            HintAssist.SetHint(textBox, SimpleRecord.GenerateTitle());

            return new[]
            {
                textBox
            };
        }

        private void Add(object query, Window window)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand((string)query, connection))
                {
                    object result = command.ExecuteScalar(); // проверка на длину поля и его уникальности
                    ((SimpleRecord)_sourceRecord).Name = ((SimpleRecord)Record).Name;
                    if (int.TryParse(result?.ToString(), out var id))
                        ((SimpleRecord)_sourceRecord).SetId(id);
                    window.Close();
                }
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