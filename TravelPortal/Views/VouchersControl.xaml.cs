using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для VouchersControl.xaml
    /// </summary>
    public partial class VouchersControl : UserControl
    {
        public VouchersControl()
        {
            InitializeComponent();
            DataContext = new VouchersViewModel();
            ((VouchersViewModel) DataContext).MessageBoxDisplayRequested +=
                (sender, e) => { MessageBox.Show(e.Text, e.Title); };

            ((VouchersViewModel) DataContext).CollectionChanged += (sender, e) =>
            {
                if (VoucherGrid.Columns.Count > 0 && e.Collection.Count > 0 &&
                    e.Collection[0] is Voucher)
                    VoucherGrid.Columns[0].Visibility =
                        e.Collection.Count(item =>
                            ((Voucher) item).CanCancelVoucher ==
                            Visibility.Visible) > 0
                            ? Visibility.Visible
                            : Visibility.Collapsed;
            };
        }


        private void VouchersControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Устанавливаем фильтр записей (а при его изменении загружаются данные из БД)
            // после загрузки элемента управления, а не в конструкторе,
            // для того, чтобы были подключены все события ViewModel,
            // в том числе событие открытия окна сообщения (при ошибке получения записей из БД),
            // вызов которого в конструкторе вызовет ошибку.
            ((VouchersViewModel) DataContext).SetDefaultFilter();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender,
            DataGridAutoGeneratingColumnEventArgs e)
        {
            e.Column.Header = Voucher.GenerateTitle(e.PropertyName);
            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";

            if (e.PropertyName == nameof(Voucher.CanCancelVoucher))
            {
                e.Column = new DataGridTemplateColumn
                {
                    CellTemplate =
                        (DataTemplate) Resources["ButtonDataTemplateColumn"]
                };
            }
        }

        private void CancelVoucher_Click(object sender, RoutedEventArgs e)
        {
            ((VouchersViewModel) DataContext).CancelCommand.Execute(null);
        }
    }
}