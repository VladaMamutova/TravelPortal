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
        private Customer _sourceCustomer;
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

        public List<string> StatusCollection { get; }

        public Visibility AddVoucherVisibility { get; }
        public Visibility UpdateCustomerVisibility { get; }
        public bool UpdateCustomer { get; }

        public RelayCommand AddVoucherCommand
        {
            get
            {
                return new RelayCommand(
                    e =>
                    {
                        try
                        {
                            MainTables.Execute(
                                Queries.MainTables.InsertVoucher(_routeId,
                                    Customer));
                            _owner.Hide();
                        }
                        catch (Exception ex)
                        {
                            OnMessageBoxDisplayRequest(
                                "Ошибка при оформлении путёвки",
                                ex is PostgresException pex
                                    ? pex.MessageText
                                    : ex.Message);
                        }
                    },
                    o => Customer.IsReadyToInsert());
            }
        }

        public RelayCommand UpdateCustomerCommand
        {
            get
            {
                return new RelayCommand(
                    e =>
                    {
                        try
                        {
                            MainTables.ExecuteAddUpdateQuery(
                                Queries.MainTables.UpdateCustomer(Customer));
                            _owner.Hide();
                        }
                        catch (Exception ex)
                        {
                            OnMessageBoxDisplayRequest(
                                "Ошибка при изменении данных клиента",
                                ex is PostgresException pex
                                    ? pex.MessageText
                                    : ex.Message);
                        }
                    },
                    o => Customer.IsReadyToInsert() &&
                         !Customer.Equals(_sourceCustomer));
            }
        }

        public VoucherRecordViewModel(int routeId, Customer customer, Window owner)
        {
            _owner = owner;
            _routeId = routeId;
            StatusCollection =
                Dictionaries.GetNameList(DictionaryKind.Status);
            _sourceCustomer = customer;
            Customer = new Customer(customer);
            if (Customer.Empty.Equals(customer))
            {
                AddVoucherVisibility = Visibility.Visible;
                UpdateCustomerVisibility = Visibility.Collapsed;
                UpdateCustomer = false;
            }
            else
            {
                AddVoucherVisibility = Visibility.Collapsed;
                UpdateCustomerVisibility = Visibility.Visible;
                UpdateCustomer = true;
            }
        }
    }
}