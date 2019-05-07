using System.Windows;
using System.Windows.Controls;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeesControl.xaml
    /// </summary>
    public partial class EmployeesControl : UserControl
    {
        public EmployeesControl(Window owner)
        {
            InitializeComponent();
            DataContext = new EmployeeViewModel(owner);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }
    }
}
