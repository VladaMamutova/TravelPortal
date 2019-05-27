using System;
using System.Collections.Generic;
using System.Windows;
using Npgsql;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class VoucherRecordViewModel: ViewModelBase
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
                    MainTables.Execute(Queries.MainTables.InsertVoucher(_routeId, Customer));
                    _owner.Hide();
                }
                catch (Exception ex)
                {
                    OnMessageBoxDisplayRequest("Ошибка при оформлении путёвки",
                        ex is PostgresException pex
                            ? pex.MessageText
                            : ex.Message);
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
    }
}