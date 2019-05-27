using System;
using System.Collections.ObjectModel;
using Npgsql;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {
        private Customer _selectedItem;
        public Customer SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private string _selectedFio;
        public string SelectedFio
        {
            get => _selectedFio;
            set
            {
                _selectedFio = value;
                OnPropertyChanged(nameof(SelectedFio));
            }
        }

        private string _selectedPhoneNumber;
        public string SelectedPhoneNumber
        {
            get => _selectedPhoneNumber;
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged(nameof(SelectedPhoneNumber));
            }
        }

        private ObservableCollection<Customer> _collection;
        public ObservableCollection<Customer> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        public CustomerViewModel()
        {
            Collection = MainTables.GetCustomers();
        }

        private RelayCommand _filterCommand;
        public RelayCommand FilterCommand
        {
            get
            {
                _filterCommand = _filterCommand ??
                                 (_filterCommand =
                                     new RelayCommand(FilterCustomers));
                return _filterCommand;
            }
        }

        void FilterCustomers(object o)
        {
            try
            {
                Customer customer = new Customer(Customer.Empty)
                {
                    Name = SelectedFio,
                    Phone = SelectedPhoneNumber
                };

                Collection = MainTables.GetCustomers(
                        Queries.MainTables.FilterCustomers(customer));
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest("Ошибка при фильтрации записей",
                    ex is PostgresException pex ? pex.MessageText : ex.Message);
            }
        }
    }
}