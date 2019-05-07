using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.Database;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class EmployeeViewModel
    {
        private Employee _selectedItem;
        public Employee SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<Employee> _collection;
        public ObservableCollection<Employee> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        public int Count => Collection?.Count ?? 0;
        private Window _owner;

        public EmployeeViewModel(Window owner)
        {
            _owner = owner;
            Collection = Customers.GetEmployees();
        }

        // commands!

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