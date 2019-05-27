using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RoutesControl.xaml
    /// </summary>
    public partial class RoutesControl : UserControl
    {
        private readonly Window _owner;
        public RoutesControl(Window owner)
        {
            InitializeComponent();
            _owner = owner;
            DataContext = new RoutesViewModel();
            ((RoutesViewModel)DataContext).MessageBoxDisplayRequested +=
                (sender, e) => { CustomMessageBox.Show(e.Title, e.Text); };
            ((RoutesViewModel)DataContext).DialogDisplayRequested +=
                (sender, e) =>
                {
                    if(e.Record is Route route)
                        new RouteRecordDialog(route) {Owner = owner}.ShowDialog();
                };
        }

        private void RoutesControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((RoutesViewModel)DataContext).LoadFromDb();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == nameof(Route.HotelPrice) ||
                e.PropertyName == nameof(Route.TransportPrice))
            {
                e.Column.Visibility = Visibility.Collapsed;
                return;
            }

            e.Column.Header = Route.GenerateTitle(e.PropertyName);

            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";

            if (e.PropertyName == nameof(Route.Name))
                e.Column.DisplayIndex = 1;

            if (e.PropertyType == typeof(double) &&
                e.Column is DataGridTextColumn textColumn)
                textColumn.Binding.StringFormat = "N2";

            if (e.PropertyName == nameof(Route.CanAddVoucher))
            {
                e.Column = new DataGridTemplateColumn
                    { CellTemplate = (DataTemplate)Resources["ButtonDataTemplateColumn"] };
            };
        }

        private void AddVoucher_Click(object sender, RoutedEventArgs e)
        {
            var view = new VoucherRecordDialog(((RoutesViewModel)DataContext).SelectedItem){Owner = _owner};
            view.ShowDialog();
        }

        private void RouteGrid_OnMouseClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(!(DataContext is RoutesViewModel model))
                return;
            if (model.SelectedItem != null)
            {
                int id = model.SelectedItem.GetId();
                var view =
                    new RouteRecordDialog(model.SelectedItem) {Owner = _owner};
                view.ShowDialog();
                model.FilterCommand.Execute(null);
                model.SelectedItem =
                    model.Collection.FirstOrDefault(
                        route => route.GetId() == id);
            }
        }
        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid)sender).SelectedItem != null)
                ((DataGrid)sender).ScrollIntoView(((DataGrid)sender).SelectedItem);
        }
    }
}
