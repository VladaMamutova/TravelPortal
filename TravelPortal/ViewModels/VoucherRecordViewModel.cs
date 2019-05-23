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
    public class VoucherRecordViewModel: ViewModelBase, INotifyPropertyChanged
    {
        private readonly Window _owner;
        private readonly int _routeId;
        private Customer _customer;
        public Customer Customer
        {
            get => _customer;
            set
            {
                _customer = value;
                OnPropertyChanged(nameof(Customer));
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
                    OnMessageBoxDisplayRequest("Ошибка при оформлении путёвки",
                        exception.Message);
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
                Dictionaries.GetNameList(DictionaryKind.Status);
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