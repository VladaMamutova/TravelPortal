using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class CustomerViewModel : ViewModelBase, INotifyPropertyChanged
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

        private Window _owner;

        public CustomerViewModel(Window owner)
        {
            _owner = owner;
            Collection = MainTables.GetCustomers();
        }

         private RelayCommand _filterCommand;
        public RelayCommand FilterCommand
        {
            get
            {
                _filterCommand = _filterCommand ??
                              (_filterCommand = new RelayCommand(obj =>
                              {
                                  try
                                  {
                                      Customer customer = new Customer
                                      {
                                         Fio = SelectedFio,
                                         Phone =  SelectedPhoneNumber
                                      };

                                      Collection =
                                          MainTables.GetCustomers(
                                              Queries.MainTables
                                                  .FilterCustomers(customer));
                                  }
                                  catch (Exception ex)
                                  {
                                      OnMessageBoxDisplayRequest("Ошибка при фильтрации записей", ex.Message);
                                  }
                              }));
                return _filterCommand;
            }
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