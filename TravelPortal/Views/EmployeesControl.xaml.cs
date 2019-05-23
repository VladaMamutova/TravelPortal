using System;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.DataAccessLayer;
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
            ((EmployeeViewModel) DataContext).DialogDisplayRequested +=
                (sender, e) =>
                {
                    new AddUserDialog(e.Record) {Owner = owner}.ShowDialog();
                };
            ((EmployeeViewModel)DataContext).MessageBoxDisplayRequested +=
                (sender, e) => { MessageBox.Show(e.Text, e.Title); };
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = User.GenerateTitle(e.PropertyName);
            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
                ((DataGrid)sender).ScrollIntoView(((DataGrid)sender).SelectedItem);
        }
    }
}
