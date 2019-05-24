using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class EmployeeViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private User _selectedItem;
        public User SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<User> _collection;

        public ObservableCollection<User> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        private Window _owner;

        public EmployeeViewModel(Window owner)
        {
            _owner = owner;
            Collection = new ObservableCollection<User>();
            UpdateCollection();
        }

        private RelayCommand _addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                _addCommand = _addCommand ??
                                   (_addCommand = new RelayCommand(obj =>
                                   {
                                       User newUser = new User(User.Empty);
                                       OnDialogDisplayRequest(newUser);
                                       UpdateCollection();
                                       SelectedItem =
                                           Collection.SingleOrDefault(
                                               i => i.GetId() == newUser.GetId());
                                   }));
                return _addCommand;
            }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                _deleteCommand = _deleteCommand ??
                              (_deleteCommand = new RelayCommand(obj =>
                              {
                                  try
                                  {
                                      MainTables.Execute(
                                          Queries.DeleteUser(SelectedItem.GetId()));
                                  }
                                  catch (Exception e)
                                  {
                                      OnMessageBoxDisplayRequest("Ошибка удаления сотрудника", e.Message);
                                  }
                                  UpdateCollection();
                                  SelectedItem = null;
                              }, o => SelectedItem != null));
                return _deleteCommand;
            }
        }

        public void UpdateCollection()
        {
            Collection.Clear();
            try
            {
                var users = MainTables.GetUsers();
                foreach (var user in users)
                    Collection.Add(user);
            }
            catch (Exception e)
            {
                OnMessageBoxDisplayRequest("Ошибка получения данных", e.Message);
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