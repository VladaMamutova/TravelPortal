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
        public UIElement[] InputBoxes { get; }
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

            InputBoxes = GenerateInputBoxes(dictionary);
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

        private UIElement[] GenerateInputBoxes(DictionaryKind dictionary)
        {
            List<UIElement> controls = new List<UIElement>();
            TextBox textBox;
            ComboBox comboBox;
            StackPanel stackPanel;
            Grid content;
            switch (dictionary)
            {
                case DictionaryKind.Hotel:
                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Hotel.Name))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox, Hotel.GenerateTitle(nameof(Record.Name)));
                    controls.Add(textBox);

                    comboBox = new ComboBox {Margin = new Thickness(0, 10, 0, 0)};
                    
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(Record) + '.' + nameof(Hotel.City))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Hotel.GenerateTitle(nameof(Hotel.City)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.City);
                    controls.Add(comboBox);

                    stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 20, 0, 0)
                    };
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = Hotel.GenerateTitle(nameof(Hotel.Type)) + ":",
                        Margin = new Thickness(0, 0, 10, 0)
                    });
                    RatingBar ratingBar = new RatingBar {Max = 5};
                    ratingBar.SetBinding(RatingBar.ValueProperty,
                        new Binding(nameof(Record) + '.' + nameof(Hotel.Type))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    stackPanel.Children.Add(ratingBar);
                    controls.Add(stackPanel);
                    break;
                case DictionaryKind.Ticket:
                    comboBox = new ComboBox();
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(Record) + '.' + nameof(Ticket.Name))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Ticket.GenerateTitle(nameof(Ticket.Name)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.Transport);
                    controls.Add(comboBox);

                    content = new Grid { Margin = new Thickness(0, 10, 0, 0) };
                    content.ColumnDefinitions.Add(new ColumnDefinition());
                    content.ColumnDefinitions.Add(new ColumnDefinition
                        {Width = GridLength.Auto});
                    content.ColumnDefinitions.Add(new ColumnDefinition());

                    comboBox = new ComboBox();
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(Record) + '.' + nameof(Ticket.From))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Ticket.GenerateTitle(nameof(Ticket.From)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.City);
                    comboBox.SetValue(Grid.ColumnProperty, 0);
                    content.Children.Add(comboBox);

                    TextBlock textBlock = new TextBlock {Width = 10};
                    textBlock.SetValue(Grid.ColumnProperty, 1);
                    content.Children.Add(textBlock);

                    comboBox = new ComboBox();
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(Record) + '.' + nameof(Ticket.To))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Ticket.GenerateTitle(nameof(Ticket.To)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.City);
                    comboBox.SetValue(Grid.ColumnProperty, 2);
                    content.Children.Add(comboBox);
                    controls.Add(content);

                    textBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Ticket.Cost))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox, Ticket.GenerateTitle(nameof(Ticket.Cost)));
                    controls.Add(textBox);
                    break;
                case DictionaryKind.Agency:
                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.Name))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox, Agency.GenerateTitle(nameof(Agency.Name)));
                    controls.Add(textBox);

                    content = new Grid { Margin = new Thickness(0, 10, 0, 0) };
                    content.ColumnDefinitions.Add(new ColumnDefinition());
                    content.ColumnDefinitions.Add(new ColumnDefinition
                    { Width = GridLength.Auto });

                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.Registration))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Registration)));
                    textBox.SetValue(Grid.ColumnProperty, 0);
                    content.Children.Add(textBox);

                    DatePicker datePicker = new DatePicker
                        {Margin = new Thickness(10, 0, 0, 0)};
                    datePicker.SetBinding(DatePicker.SelectedDateProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.Date))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(datePicker,
                        Agency.GenerateTitle(nameof(Agency.Date)));
                    datePicker.SetValue(Grid.ColumnProperty, 1);
                    content.Children.Add(datePicker);
                    controls.Add(content);

                    content = new Grid { Margin = new Thickness(0, 10, 0, 0) };
                    content.ColumnDefinitions.Add(new ColumnDefinition
                        {Width = GridLength.Auto});
                    content.ColumnDefinitions.Add(new ColumnDefinition());

                    comboBox = new ComboBox {Margin = new Thickness(0,0,10,0), MinWidth = 60};
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.City))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Agency.GenerateTitle(nameof(Agency.City)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.City);
                    comboBox.SetValue(Grid.ColumnProperty, 0);
                    content.Children.Add(comboBox);

                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.Address))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Address)));
                    textBox.SetValue(Grid.ColumnProperty, 1);
                    content.Children.Add(textBox);
                    controls.Add(content);

                    comboBox = new ComboBox {Margin = new Thickness(0,10,0,0)};
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.Ownership))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Agency.GenerateTitle(nameof(Agency.Ownership)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.Ownership);
                    controls.Add(comboBox);
                    
                    textBox = new TextBox {Margin = new Thickness(0, 10, 0, 0)};
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Agency.Phone))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Phone)));
                    controls.Add(textBox);
                    break;
                default:
                {
                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(Record) + '.' + nameof(Record.Name))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox, SimpleRecord.GenerateTitle());
                    controls.Add(textBox);
                    break;
                }
            }

            return controls.ToArray();
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
                                .ExecuteScalar(); // проверка на длину поля и его уникальности
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