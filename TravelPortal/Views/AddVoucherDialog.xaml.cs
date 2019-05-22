using System.Windows;
using System.Windows.Input;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для AddVoucherDialog.xaml
    /// </summary>
    public partial class AddVoucherDialog : Window
    {
        public AddVoucherDialog(Route route)
        {
            InitializeComponent();
            HotelName.Text = route.Hotel;
            Date.Text = route.Date.ToShortDateString();
            DataContext = new VoucherRecordViewModel(route.GetId(), this);
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
