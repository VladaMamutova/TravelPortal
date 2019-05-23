using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    public class UserRecordViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private readonly Window _owner;
        public List<string> Roles { get; }
        public List<string> AgencyCollection { get; }

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private string _selectedRole;
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                if (_selectedRole == Roles[1])
                {
                    User.Role = Models.Roles.Supervisor;
                    AgencyVisibility = Visibility.Collapsed;
                }
                else
                {
                    if (_selectedRole == Roles[0])
                    {
                        User.Role = Models.Roles.Employee;
                        AgencyVisibility = Visibility.Visible;
                    }
                    else User.Role = Models.Roles.None;
                }

                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        private Visibility _agencyVisibility;
        public Visibility AgencyVisibility
        {
            get => _agencyVisibility;
            set
            {
                _agencyVisibility = value;
                OnPropertyChanged(nameof(AgencyVisibility));
            }
        }

        public RelayCommand AddCommand => new RelayCommand(
            e =>
            {
                try
                {
                    object result = MainTables.ExecuteAddUpdateQuery(Queries.AddUser(User,
                        ((AddUserDialog) _owner).PasswordBox.Password));
                    if (int.TryParse(result?.ToString(), out var id))
                        User.SetId(id);
                    _owner.Close();
                }
                catch (Exception exception)
                {
                    OnMessageBoxDisplayRequest(
                        "Ошибка при добавлении пользователя",
                        exception.Message);
                }
            },
            o => User.IsReadyToInsert());

        public RelayCommand DeleteCommand => new RelayCommand(null);

        public UserRecordViewModel(User user, Window owner)
        {
            _owner = owner;
            Roles = new List<string> {"Сотрудник", "Супервайзер"};
            User = user;
            AgencyCollection = Dictionaries.GetNameList(DictionaryKind.Agency);
            SelectedRole = Roles[0];
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