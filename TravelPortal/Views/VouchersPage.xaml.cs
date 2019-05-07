using System.Windows.Controls;
using TravelPortal.Database;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для VouchersPage.xaml
    /// </summary>
    public partial class VouchersPage : Page
    {
        public VouchersPage()
        {
            InitializeComponent();
            //DataContext = new VoucherViewModel(this);
        }

        private void ClientFio_Changed(object sender, TextChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = string.IsNullOrWhiteSpace(ClientFio.Text) ?
                Vouchers.GetVouchers() : Vouchers.SearchByClientFio(ClientFio.Text);
        }

        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = StatusBox.SelectedIndex == -1
                ? Vouchers.GetVouchers()
                : Vouchers.SearchByStatus(StatusBox.SelectedItem.ToString());
        }
    }
}
