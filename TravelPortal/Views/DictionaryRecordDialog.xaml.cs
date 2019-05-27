using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionaryRecordDialog.xaml
    /// </summary>
    public partial class DictionaryRecordDialog : Window
    {
        public DictionaryRecordDialog(DictionaryKind dictionary, SimpleRecord record)
        {
            InitializeComponent();

            DictionaryRecordViewModel viewModel =
                new DictionaryRecordViewModel(dictionary, this, record);
            InputBoxes.ItemsSource = GenerateContent(dictionary, record);
            DataContext = viewModel;
        }

        private UIElement[] GenerateContent(DictionaryKind dictionary, SimpleRecord record)
        {
            List<UIElement> inputs = new List<UIElement>();
            TextBox textBox;
            ComboBox comboBox;
            Grid content;
            switch (dictionary)
            {
                case DictionaryKind.Hotel:
                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Hotel.Name))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox, Hotel.GenerateTitle(nameof(Hotel.Name)));
                    inputs.Add(textBox);

                    comboBox = new ComboBox { Margin = new Thickness(0, 10, 0, 0) };

                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Hotel.City))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Hotel.GenerateTitle(nameof(Hotel.City)));
                    comboBox.ItemsSource = Hotel.Empty.Equals(record)
                        ? Dictionaries.GetNameList(DictionaryKind.City)
                        : new List<string> {((Hotel) record).City};
                    inputs.Add(comboBox);

                    var stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Margin = new Thickness(0, 20, 0, 0)
                    };
                    stackPanel.Children.Add(new TextBlock
                    {
                        Text = Hotel.GenerateTitle(nameof(Hotel.Type)) + ":",
                        Margin = new Thickness(0, 0, 10, 0)
                    });
                    RatingBar ratingBar = new RatingBar { Max = 5 };
                    ratingBar.SetBinding(RatingBar.ValueProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Hotel.Type))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    stackPanel.Children.Add(ratingBar);
                    inputs.Add(stackPanel);
                    break;
                case DictionaryKind.Ticket:
                    comboBox = new ComboBox();
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Ticket.Name))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Ticket.GenerateTitle(nameof(Ticket.Name)));
                    comboBox.ItemsSource = Ticket.Empty.Equals(record)
                        ? Dictionaries.GetNameList(DictionaryKind.Transport)
                        : new List<string> {((Ticket) record).Name};
                    inputs.Add(comboBox);

                    content = new Grid { Margin = new Thickness(0, 10, 0, 0) };
                    content.ColumnDefinitions.Add(new ColumnDefinition());
                    content.ColumnDefinitions.Add(new ColumnDefinition
                    { Width = GridLength.Auto });
                    content.ColumnDefinitions.Add(new ColumnDefinition());

                    comboBox = new ComboBox();
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) +
                                    '.' + nameof(Ticket.From))
                        {
                            UpdateSourceTrigger =
                                UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox,
                        Ticket.GenerateTitle(nameof(Ticket.From)));
                    comboBox.ItemsSource = Ticket.Empty.Equals(record)
                        ? Dictionaries.GetNameList(DictionaryKind.City)
                        : new List<string> {((Ticket) record).From};
                    comboBox.SetValue(Grid.ColumnProperty, 0);
                    content.Children.Add(comboBox);

                    TextBlock textBlock = new TextBlock { Width = 10 };
                    textBlock.SetValue(Grid.ColumnProperty, 1);
                    content.Children.Add(textBlock);

                    comboBox = new ComboBox();
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) +
                                    '.' + nameof(Ticket.To))
                        {
                            UpdateSourceTrigger =
                                UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox,
                        Ticket.GenerateTitle(nameof(Ticket.To)));
                    comboBox.ItemsSource = Ticket.Empty.Equals(record)
                        ? Dictionaries.GetNameList(DictionaryKind.City)
                        : new List<string> {((Ticket) record).To};
                    comboBox.SetValue(Grid.ColumnProperty, 2);
                    content.Children.Add(comboBox);
                    inputs.Add(content);

                    textBox = new TextBox { Margin = new Thickness(0, 10, 0, 0) };
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) +
                                    '.' + nameof(Ticket.Cost))
                        {
                            UpdateSourceTrigger =
                                UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Ticket.GenerateTitle(nameof(Ticket.Cost)));
                    inputs.Add(textBox);
                    break;
                case DictionaryKind.Agency:
                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) +
                                    '.' + nameof(Agency.Name))
                        {
                            UpdateSourceTrigger =
                                UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Name)));
                    inputs.Add(textBox);

                    content = new Grid { Margin = new Thickness(0, 10, 0, 0) };
                    content.ColumnDefinitions.Add(new ColumnDefinition());
                    content.ColumnDefinitions.Add(new ColumnDefinition
                    { Width = GridLength.Auto });

                    textBox = new TextBox();
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Agency.Registration))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Registration)));
                    textBox.SetValue(Grid.ColumnProperty, 0);
                    content.Children.Add(textBox);

                    DatePicker datePicker = new DatePicker
                    { Margin = new Thickness(10, 0, 0, 0) };
                    datePicker.SetBinding(DatePicker.SelectedDateProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Agency.Date))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(datePicker,
                        Agency.GenerateTitle(nameof(Agency.Date)));
                    datePicker.SetValue(Grid.ColumnProperty, 1);
                    content.Children.Add(datePicker);
                    inputs.Add(content);

                    content = new Grid { Margin = new Thickness(0, 10, 0, 0) };
                    content.ColumnDefinitions.Add(new ColumnDefinition
                    { Width = GridLength.Auto });
                    content.ColumnDefinitions.Add(new ColumnDefinition());

                    comboBox = new ComboBox { Margin = new Thickness(0, 0, 10, 0), MinWidth = 60 };
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Agency.City))
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
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Agency.Address))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Address)));
                    textBox.SetValue(Grid.ColumnProperty, 1);
                    content.Children.Add(textBox);
                    inputs.Add(content);

                    comboBox = new ComboBox { Margin = new Thickness(0, 10, 0, 0) };
                    comboBox.SetBinding(Selector.SelectedItemProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Agency.Ownership))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(comboBox, Agency.GenerateTitle(nameof(Agency.Ownership)));
                    comboBox.ItemsSource = Dictionaries.GetNameList(DictionaryKind.Ownership);
                    inputs.Add(comboBox);

                    textBox = new TextBox { Margin = new Thickness(0, 10, 0, 0), MaxLength = 12};
                    textBox.SetBinding(TextBox.TextProperty,
                        new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(Agency.Phone))
                        {
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Mode = BindingMode.TwoWay
                        });
                    HintAssist.SetHint(textBox,
                        Agency.GenerateTitle(nameof(Agency.Phone)));
                    inputs.Add(textBox);
                    break;
                default:
                    {
                        textBox = new TextBox();
                        textBox.SetBinding(TextBox.TextProperty,
                            new Binding(nameof(DictionaryRecordViewModel.Record) + '.' + nameof(DictionaryRecordViewModel.Record.Name))
                            {
                                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                                Mode = BindingMode.TwoWay
                            });
                        HintAssist.SetHint(textBox, SimpleRecord.GenerateTitle());
                        inputs.Add(textBox);
                        break;
                    }
            }

            return inputs.ToArray();
        }

        private void Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
