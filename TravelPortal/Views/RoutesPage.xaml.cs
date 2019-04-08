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
        }

        private void RouteName_Changed(object sender, TextChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(RouteName.Text) ?
                Routes.GetAll() : Routes.Search(RouteName.Text);
        }

        private void StartDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(StartDate.Text) ?
                Routes.GetAll() : Routes.Search(StartDate.DisplayDate);
        }

        private void Duration_Changed(object sender, TextChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(Duration.Text) ?
                Routes.GetAll() : Routes.Search(Convert.ToInt32(Duration.Text));
        }

        private void AddRoute_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
