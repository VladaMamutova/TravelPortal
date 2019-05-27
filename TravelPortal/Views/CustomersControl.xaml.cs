using System;
using System.Linq;
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
        private readonly Window _owner;
        public CustomersControl(Window owner)
        {
            InitializeComponent();
            _owner = owner;
            DataContext = new CustomerViewModel();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = Customer.GenerateTitle(e.PropertyName);
            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";
            if (e.PropertyName == nameof(Customer.Name))
                e.Column.DisplayIndex = 1;
        }

        private void CustomerGrid_OnMouseClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(DataContext is CustomerViewModel model))
                return;
            string name = model.SelectedItem.Name;
            var view = new VoucherRecordDialog(Route.Empty,
                    ((CustomerViewModel) DataContext).SelectedItem)
                {Owner = _owner};
            view.ShowDialog();
            model.FilterCommand.Execute(null);
            model.SelectedItem =
                model.Collection.FirstOrDefault(
                    route => route.Name == name);
        }
    }
}
