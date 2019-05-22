using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RoutesControl.xaml
    /// </summary>
    public partial class RoutesControl : UserControl
    {
        private Window _owner;
        public RoutesControl(Window owner)
        {
            InitializeComponent();
            _owner = owner;
            DataContext = new RouteViewModel(_owner);
        }

        private void RouteName_Changed(object sender, TextChangedEventArgs e)
        {

        }

        private void StartDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            //RoutesDataGrid.ItemsSource = StartDate.SelectedDate == null ?
            //    Routes.GetAll() : Routes.SearchByDate(StartDate.SelectedDate.Value);
        }

        private void Duration_Changed(object sender, TextChangedEventArgs e)
        {
            //RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(Duration.Text) ?
            //    Routes.GetAll() : Routes.SearchByDuration(Convert.ToInt32(Duration.Text));
        }

        private void AddRoute_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //RoutesDataGrid.ItemsSource = ResidenceBox.SelectedIndex == -1
            //    ? Routes.GetAll()
            //    : Routes.FilterResidence(ResidenceBox.SelectedItem.ToString());
        }

        private void TransportTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //RoutesDataGrid.ItemsSource = TransportBox.SelectedIndex == -1
            //    ? Routes.GetAll()
            //    : Routes.FilterTransport(TransportBox.SelectedItem.ToString());
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == nameof(Route.HotelPrice) ||
                e.PropertyName == nameof(Route.TransportPrice))
            {
                e.Column.Visibility = Visibility.Collapsed;
                return;
            }

            e.Column.Header = Route.GenerateTitle(e.PropertyName);

            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";

            if (e.PropertyType == typeof(double) &&
                e.Column is DataGridTextColumn textColumn)
                textColumn.Binding.StringFormat = "N2";

            if (e.PropertyName == nameof(Route.CanAddVoucher))
            {
                e.Column = new DataGridTemplateColumn
                    { CellTemplate = (DataTemplate)Resources["ButtonDataTemplateColumn"] };
            };
        }

        private void AddVoucher_Click(object sender, RoutedEventArgs e)
        {
            var view = new AddVoucherDialog(((RouteViewModel)DataContext).SelectedItem){Owner = _owner};
            view.ShowDialog();
        }
    }
}
