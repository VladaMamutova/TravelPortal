using System.Windows;
using System.Windows.Controls;
using TravelPortal.Database;

namespace TravelPortal.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Routes_Click(object sender, RoutedEventArgs e)
        {
            VouchersPage.Visibility = Visibility.Collapsed;
            RoutesPage.Visibility = Visibility.Visible;

            RoutesDataGrid.ItemsSource = Routes.GetAll();
        }

        private void Vouchers_Click(object sender, RoutedEventArgs e)
        {
            RoutesPage.Visibility = Visibility.Collapsed;
            VouchersPage.Visibility = Visibility.Visible;

            VouchersDataGrid.ItemsSource = Vouchers.GetAll();
        }

        private void AddRoute_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void RouteName_Changed(object sender, TextChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(RouteName.Text) ?
                Routes.GetAll() : Routes.Search(RouteName.Text);
        }

        private void ClientFio_Changed(object sender, TextChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = string.IsNullOrWhiteSpace(ClientFio.Text) ?
                Vouchers.GetAll() : Vouchers.Search(ClientFio.Text);
        }

        private void StartDate_Changed(object sender, SelectionChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(StartDate.Text) ?
                Routes.GetAll() : Routes.Search(StartDate.DisplayDate);
        }

        private void Duration_Changed(object sender, TextChangedEventArgs e)
        {
            RoutesDataGrid.ItemsSource = string.IsNullOrWhiteSpace(StartDate.Text) ?
                Routes.GetAll() : Routes.Search(Duration.Text);
        }

        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = StatusBox.SelectedIndex == -1
                ? Vouchers.GetAll()
                : Vouchers.SearchByStatus(
                    ((ComboBoxItem) (StatusBox.Items[StatusBox.SelectedIndex]))
                    .Content.ToString());
        }
    }
}
