using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

        public string CaptionText { get; }
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
                            Regex regex = new Regex(@"^\+7(9[0-9]{2}|495|499)\d{7}$");
                            if (!regex.IsMatch(Customer.Phone))
                                throw new Exception("Номер должен быть введён в международном формате (+7), " +
                                                    "затем должен быть указан префикс региона и оператора " +
                                                    "(900-999, для городских - 495 или 499), далее семь цифр.");
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
                            Regex regex = new Regex(@"^\+7(9[0-9]{2}|495|499)\d{7}$");
                            if (!regex.IsMatch(Customer.Phone))
                                throw new Exception("Номер должен быть введён в международном формате (+7), " +
                                                    "затем должен быть указан префикс региона и оператора " +
                                                    "(900-999, для городских - 495 или 499), далее семь цифр.");
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
                CaptionText = "Оформление путёвки";
                AddVoucherVisibility = Visibility.Visible;
                UpdateCustomerVisibility = Visibility.Collapsed;
                UpdateCustomer = false;
            }
            else
            {
                CaptionText = "Просмотр данных клиента";
                AddVoucherVisibility = Visibility.Collapsed;
                UpdateCustomerVisibility = Visibility.Visible;
                UpdateCustomer = true;
            }
        }
    }
}