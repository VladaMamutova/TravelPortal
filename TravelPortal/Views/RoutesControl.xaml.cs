using System;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.Database;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RoutesControl.xaml
    /// </summary>
    public partial class RoutesControl : UserControl
    {
        public RoutesControl(Window owner)
        {
            InitializeComponent();
            DataContext = new RouteViewModel(owner);
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

        }
    }
}
