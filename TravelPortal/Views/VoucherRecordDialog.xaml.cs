using System.Windows;
using System.Windows.Input;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для AddVoucherDialog.xaml
    /// </summary>
    public partial class VoucherRecordDialog : Window
    {
        public VoucherRecordDialog(Route route)
        {
            InitializeComponent();
            HotelName.Text = route.Name;
            Date.Text = route.Date.ToShortDateString();
            DataContext = new VoucherRecordViewModel(route.GetId(), this);
            ((VoucherRecordViewModel)DataContext).MessageBoxDisplayRequested += (sender, e) =>
            {
                CustomMessageBox.Show(e.Title, e.Text);
            };
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
