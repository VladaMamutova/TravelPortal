using System.Windows;
using System.Windows.Controls;
using TravelPortal.Database;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для VouchersControl.xaml
    /// </summary>
    public partial class VouchersControl : UserControl
    {
        public VouchersControl(Window owner)
        {
            InitializeComponent();
        }

        private void ClientFio_Changed(object sender, TextChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = string.IsNullOrWhiteSpace(ClientFio.Text) ?
                Vouchers.GetAll() : Vouchers.SearchByClientFio(ClientFio.Text);
        }

        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = StatusBox.SelectedIndex == -1
                ? Vouchers.GetAll()
                : Vouchers.SearchByStatus(StatusBox.SelectedItem.ToString());
        }
    }
}
