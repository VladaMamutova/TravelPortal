using System;
using System.Collections.ObjectModel;
using System.Linq;
using Npgsql;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class UsersViewModel : ViewModelBase
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

        public UsersViewModel()
        {
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

        private RelayCommand _updateCommand;
        public RelayCommand UpdateCommand
        {
            get
            {
                _updateCommand = _updateCommand ??
                              (_updateCommand = new RelayCommand(obj =>
                              {
                                  User user = SelectedItem;
                                  OnDialogDisplayRequest(SelectedItem);
                                  UpdateCollection();
                                  SelectedItem =
                                      Collection.SingleOrDefault(
                                          i => i.GetId() == user.GetId());
                              }, o => SelectedItem != null && SelectedItem.Role != Roles.Admin));
                return _updateCommand;
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
                                          Queries.Users.DeleteUser(SelectedItem.GetId()));
                                  }
                                  catch (Exception e)
                                  {
                                      OnMessageBoxDisplayRequest("Ошибка удаления сотрудника", e is PostgresException pex
                                          ? pex.MessageText
                                          : e.Message);
                                  }
                                  UpdateCollection();
                                  SelectedItem = null;
                              }, o => SelectedItem != null && SelectedItem.Role != Roles.Admin));
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
                OnMessageBoxDisplayRequest("Ошибка получения данных",
                    e is PostgresException pex
                        ? pex.MessageText
                        : e.Message);
            }
        }
    }
}