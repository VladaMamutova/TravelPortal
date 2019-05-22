using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class VoucherRecordViewModel:INotifyPropertyChanged
    {
        private Window _owner;
        private int _routeId;
        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Voucher));
            }
        }

        public bool IsInProgress { get; set; }
        public List<string> StatusCollection { get; }

        public RelayCommand AddVoucher => new RelayCommand(
            e =>
            {
                try
                {
                    MainTables.Execute(Queries.InsertVoucher(_routeId, Customer));
                    _owner.Hide();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message,
                        "Ошибка при оформлении путёвки");
                }
            },
            o => Customer.IsReadyToInsert());

        public RelayCommand CheckCustomer => new RelayCommand(
            e => { IsInProgress = true; },
            o => !string.IsNullOrWhiteSpace(Customer.Fio) ||
                 !string.IsNullOrWhiteSpace(Customer.Phone));

        public VoucherRecordViewModel(int routeId, Window owner)
        {
            _owner = owner;
            Customer = new Customer();
            _routeId = routeId;
            IsInProgress = false;
            StatusCollection =
                Dictionaries.GetNameView(Queries.SelectStatusNameView);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}