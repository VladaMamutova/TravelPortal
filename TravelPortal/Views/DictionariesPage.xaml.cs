using System.Windows;
using System.Windows.Controls;
using TravelPortal.Models;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionariesPage.xaml
    /// </summary>
    public partial class DictionariesPage : Page
    {
        public DictionariesPage(Window owner)
        {
            InitializeComponent();
            DataContext = new DictionariesViewModel(owner);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == nameof(Hotel.Type))
                e.Column = new DataGridTemplateColumn
                { CellTemplate = (DataTemplate)Resources["RatingBarDataTemplate"] };

            if (e.PropertyType == typeof(System.DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";

            e.Column.Header =
                ((DictionaryViewModel)Dictionaries.SelectedItem).GenerateTitleFunc
                .Invoke(e.PropertyName);

            if (e.PropertyName == nameof(SimpleRecord.Name))
                e.Column.DisplayIndex = 0;
        }

        public void InitializeMenu()
        {
            // В зависимости от роли пользователя будут
            // генерироваться соответствующие пункты меню.

            //DictionaryCommands commands = new DictionaryCommands();
            //Dictionaries.AddToSource(CreateDictionaryTabItem("Агенства",
            //    PackIconKind.OfficeBuilding, nameof(dictionaryTabItemViewModel.Agencies), Command(),
            //    Command1(), Agency.GenerateTitle));
            //Dictionaries.AddToSource(CreateDictionaryTabItem("Билеты на проезд",
            //    PackIconKind.Cards, nameof(dictionaryTabItemViewModel.Tickets), Command(),
            //    Command1(), Ticket.GenerateTitle));
            //Dictionaries.AddToSource(CreateDictionaryTabItem("Вид транспорта",
            //    PackIconKind.Aeroplane,
            //    nameof(dictionaryTabItemViewModel.TransportCollection),
            //    dictionaryTabItemViewModel.AddTransportCommand, dictionaryTabItemViewModel.ModifyTransportCommand, SimpleRecord.GenerateTitle));
            //Dictionaries.AddToSource(CreateDictionaryTabItem("Города",
            //    PackIconKind.City, nameof(dictionaryTabItemViewModel.Cities), Command(),
            //    Command1(), SimpleRecord.GenerateTitle));
            //TabItem tabItem = CreateDictionaryTabItem("Отели",
            //    PackIconKind.Hotel, nameof(dictionaryTabItemViewModel.Hotels), Command(),
            //    Command1(), null);

            //((DataGrid) ((Grid) tabItem.Content).Children[1]).AutoGeneratingColumn += (sender, e) =>
            //{
            //    if(e.PropertyName == nameof(Hotel.Type))
            //        e.Column = new DataGridTemplateColumn
            //            { CellTemplate = (DataTemplate)Resources["RatingBarDataTemplate"] };
            //    e.Column.Header = Hotel.GenerateTitle(e.PropertyName);
            //};
            //Dictionaries.AddToSource(tabItem);
            //Dictionaries.AddToSource(CreateDictionaryTabItem("Социальное положение",
            //    PackIconKind.TicketUser, nameof(dictionaryTabItemViewModel.Status), Command(),
            //    Command1(), SimpleRecord.GenerateTitle));
            //Dictionaries.AddToSource(CreateDictionaryTabItem("Тип собственности",
            //    PackIconKind.SecurityHome, nameof(dictionaryTabItemViewModel.Ownership), Command(),
            //    Command1(), SimpleRecord.GenerateTitle));
            //DataContext = dictionaryTabItemViewModel;

        }
    }
}

