﻿using System.Windows.Controls;
using TravelPortal.Database;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для VouchersPage.xaml
    /// </summary>
    public partial class VouchersPage : Page
    {
        public VouchersPage()
        {
            InitializeComponent();
            DataContext = new VoucherViewModel();
        }

        private void ClientFio_Changed(object sender, TextChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = string.IsNullOrWhiteSpace(ClientFio.Text) ?
                Vouchers.GetAll() : Vouchers.SearchByClientFio(ClientFio.Text);
        }

        private void StatusBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VouchersDataGrid.ItemsSource = StatusBox.SelectedIndex == -1
                ? Vouchers.GetAll()
                : Vouchers.SearchByStatus(StatusBox.SelectedItem.ToString());
        }
    }
}