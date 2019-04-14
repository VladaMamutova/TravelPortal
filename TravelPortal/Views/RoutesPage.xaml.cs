using System;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.Database;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RoutesPage.xaml
    /// </summary>
    public partial class RoutesPage : Page
    {
        public RoutesPage()
        {
            InitializeComponent();
            DataContext = new RouteViewModel();
            RoutesDataGrid.ItemsSource = Routes.GetAll();
        }

        private void RouteName_Changed(object sender, TextChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(RouteName.Text) ?
                Routes.GetAll() : Routes.Search(RouteName.Text);
        }

        private void StartDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = StartDate.SelectedDate == null ?
                Routes.GetAll() : Routes.Search(StartDate.SelectedDate.Value);
        }

        private void Duration_Changed(object sender, TextChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(Duration.Text) ?
                Routes.GetAll() : Routes.Search(Convert.ToInt32(Duration.Text));
        }

        private void AddRoute_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = ResidenceBox.SelectedIndex == -1
                ? Routes.GetAll()
                : Routes.FilterResidence(ResidenceBox.SelectedItem.ToString());
        }

        private void TransportTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = TransportBox.SelectedIndex == -1
                ? Routes.GetAll()
                : Routes.FilterTransport(TransportBox.SelectedItem.ToString());
        }
    }
}
