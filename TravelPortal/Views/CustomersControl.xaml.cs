using System;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomersControl.xaml
    /// </summary>
    public partial class CustomersControl : UserControl
    {
        public CustomersControl()
        {
            InitializeComponent();
            DataContext = new CustomerViewModel();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = Customer.GenerateTitle(e.PropertyName);
            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";
        }
    }
}
