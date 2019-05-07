using System.Windows;
using System.Windows.Controls;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersControl.xaml
    /// </summary>
    public partial class CustomersControl : UserControl
    {
        public CustomersControl(Window owner)
        {
            InitializeComponent();
            DataContext = new CustomerViewModel(owner);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
