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
        
        private readonly User _initialUser;

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

        public RelayCommand Command { get; }
        
        public string CommandText { get; }
        public string CaptionText { get; }

        public UserRecordViewModel(User user, Window owner)
        {
            _owner = owner;
            CommandText = User.Empty.Equals(user)
                ? "ДОБАВИТЬ"
                : "ИЗМЕНИТЬ";
            CaptionText = User.Empty.Equals(user)
                ? "Добавление сотрудника"
                : "Изменение сотрудника";
            _initialUser = user;
            User = new User(user);
            Roles = new List<string> {"Сотрудник", "Супервайзер"};
            AgencyCollection = Dictionaries.GetNameList(DictionaryKind.Agency);
            SelectedRole =
                User.Empty.Equals(user) || User.Role == Models.Roles.Employee
                    ? Roles[0]
                    : Roles[1];
            if (User.Empty.Equals(user))
                Command = new RelayCommand(o =>
                    {
                        Execute(Queries.AddUser(User,
                                ((AddUserDialog) _owner).PasswordBox.Password));
                    },
                    o => User.IsReadyToInsert());
           else Command = new RelayCommand(o =>
                {
                    // Проверяем пароль на корректность.
                    // Пытаемся подключиться к базе со старым логином и паролем.
                    try
                    {
                        MainTables.CheckUserPassword(_initialUser.Login,
                            ((AddUserDialog) _owner).PasswordBox.Password);
                        Execute(Queries.UpdateUser(User,
                                ((AddUserDialog)_owner).PasswordBox.Password));
                    }
                    catch
                    {
                        OnMessageBoxDisplayRequest("Неверный пароль",
                            "Пароль данного пользователя с введённым не совпадает.");
                    }
                },
               o => User.IsReadyToInsert() && !User.Equals(
                         _initialUser));
        }

        private void Execute(string query)
        {
            try
            {
                object result = MainTables.ExecuteAddUpdateQuery(query);
                if (int.TryParse(result?.ToString(), out var id))
                    User.SetId(id);
                _owner.Close();
            }
            catch (Exception e)
            {
                OnMessageBoxDisplayRequest(
                    CommandText == "ДОБАВИТЬ"
                        ? "Ошибка при добавлении пользователя"
                        : "Ошибка при изменении пользователя",
                    e.Message);
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