using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using NpgsqlTypes;
using TravelPortal.Models;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionariesPage.xaml
    /// </summary>
    public partial class DictionariesPage : Page
    {
        public DictionariesPage()
        {
            InitializeComponent();
            InitializeMenu();
        }

        public void InitializeMenu()
        {
            // В зависимости от роли пользователя будут
            // генерироваться соответствующие пункты меню.

            // Меню рядового сотрудника туристического портала.
            DictionaryViewModel dictionaryViewModel = new DictionaryViewModel();
            Dictionaries.AddToSource(CreateDictionaryTabItem("Агенства",
                PackIconKind.OfficeBuilding, nameof(dictionaryViewModel.Agencies), Command(),
                Command1(), Agency.GenerateTitle));
            Dictionaries.AddToSource(CreateDictionaryTabItem("Билеты на проезд",
                PackIconKind.Cards, nameof(dictionaryViewModel.Tickets), Command(),
                Command1(), Ticket.GenerateTitle));
            Dictionaries.AddToSource(CreateDictionaryTabItem("Вид транспорта",
                PackIconKind.Aeroplane,
                nameof(dictionaryViewModel.TransportCollection),
                Command(), Command1(), SimpleRecord.GenerateTitle));
            Dictionaries.AddToSource(CreateDictionaryTabItem("Города",
                PackIconKind.City, nameof(dictionaryViewModel.Cities), Command(),
                Command1(), SimpleRecord.GenerateTitle));
            Dictionaries.AddToSource(CreateDictionaryTabItem("Отели",
                PackIconKind.Hotel, nameof(dictionaryViewModel.Hotels), Command(),
                Command1(), Hotel.GenerateTitle));
            Dictionaries.AddToSource(CreateDictionaryTabItem("Социальное положение",
                PackIconKind.TicketUser, nameof(dictionaryViewModel.Status), Command(),
                Command1(), SimpleRecord.GenerateTitle));
            Dictionaries.AddToSource(CreateDictionaryTabItem("Тип собственности",
                PackIconKind.SecurityHome, nameof(dictionaryViewModel.Ownership), Command(),
                Command1(), SimpleRecord.GenerateTitle));
            DataContext = dictionaryViewModel;

        }

        public RelayCommand Command()
        {
            return new RelayCommand(obj =>
            {
                ((DictionaryViewModel) DataContext).Agencies.Add(
                    new Agency(1, "Rerer66", "Агенство 1", "Город 1", "hhh",
                        "Государственная", "+7981882882", new NpgsqlDate(DateTime.Now)));
            }, obj => true);
        }

        public RelayCommand Command1()
        {
            return new RelayCommand(obj =>
            {
                ((DictionaryViewModel) DataContext).Agencies[0].Registration =
                    "2";
            }, obj => true);
        }

        // привязки надо сделать
        public TabItem CreateDictionaryTabItem(string title, PackIconKind icon,
            string dataGridBinding, RelayCommand addCommand,
            RelayCommand modifyCommand, Func<string, string> generateTitleFunc)
        {
            TabItem tabItem = new TabItem();

            // TabItem.Header.
            StackPanel header = new StackPanel
                {Orientation = Orientation.Horizontal};
            header.Children.Add(new PackIcon
            {
                Kind = icon, Height = 30, Width = 30,
                Margin = new Thickness(10, 0, 10, 0)
            });
            header.Children.Add(new TextBlock
                {Text = title, VerticalAlignment = VerticalAlignment.Center});

            // TabItem.Content.
            Grid content = new Grid();
            content.RowDefinitions.Add(new RowDefinition
                {Height = GridLength.Auto});
            content.RowDefinitions.Add(new RowDefinition());

            // Панель с управляющими контролами.
            StackPanel controls = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            controls.Children.Add(new Button
            {
                Content = "Добавить", Margin = new Thickness(0, 10, 10, 0),
                Command = addCommand
            });
            controls.Children.Add(new Button
            {
                Content = "Изменить",
                Margin = new Thickness(0, 10, 10, 0),
                Command = modifyCommand
            });
            controls.SetValue(Grid.RowProperty, 0);

            // Таблица с данными.
            DataGrid table = new DataGrid {Margin = new Thickness(10)};
            table.AutoGeneratingColumn += (sender, e) =>
            {
                e.Column.Header = generateTitleFunc.Invoke(e.PropertyName);
            };
            table.SetBinding(ItemsControl.ItemsSourceProperty,
                new Binding(dataGridBinding));
            table.SetValue(Grid.RowProperty, 1);

            content.Children.Add(controls);
            content.Children.Add(table);

            // Добавляем созданные элементы в TabItem.
            tabItem.Header = header;
            tabItem.Content = content;

            return tabItem;
        }
    }
}

