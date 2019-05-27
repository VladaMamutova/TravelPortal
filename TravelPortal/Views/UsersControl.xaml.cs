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
    public partial class UsersControl : UserControl
    {
        public UsersControl(Window owner)
        {
            InitializeComponent();
            DataContext = new UsersViewModel();
            ((UsersViewModel) DataContext).DialogDisplayRequested +=
                (sender, e) =>
                {
                    new UserRecordDialog(e.Record) {Owner = owner}.ShowDialog();
                };
            ((UsersViewModel)DataContext).MessageBoxDisplayRequested +=
                (sender, e) => { CustomMessageBox.Show(e.Title, e.Text); };
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = User.GenerateTitle(e.PropertyName);
            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";
            if (e.PropertyName == nameof(User.Name))
                e.Column.DisplayIndex = 2;
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
                ((DataGrid)sender).ScrollIntoView(((DataGrid)sender).SelectedItem);
        }
    }
}
