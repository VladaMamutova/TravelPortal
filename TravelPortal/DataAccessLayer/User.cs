using System;
using TravelPortal.Models;

namespace TravelPortal.DataAccessLayer
{
    public class User : SimpleRecord
    {
        private readonly int _agencyId;
        private Roles _role;
        private string _login;
        private string _agency;
        private string _city;

        public Roles Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Agency
        {
            get => _agency;
            set
            {
                _agency = value;
                OnPropertyChanged(nameof(Agency));
            }
        }
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public int GetAgencyId() => _agencyId;

        public new static readonly User Empty;

        static User()
        {
            Empty = new User(-1, Roles.None, "", "", -1, "", "");
        }

        public User(int id, Roles role, string name, string login, int agencyId, string agency, string city) : base(id, name)
        {
            _agencyId = agencyId;
            Role = role;
            Name = name;
            Login = login;
            Agency = agency;
            City = city;
        }

        public User(User newRecord) : base(newRecord.Id, newRecord.Name)
        {
            _agencyId = newRecord.GetAgencyId();
            Role = newRecord.Role;
            Name = newRecord.Name;
            Login = newRecord.Login;
            Agency = newRecord.Agency;
            City = newRecord.City;
        }

        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Role): return "Роль";
                case nameof(Agency): return "Агенство";
                case nameof(City): return "Город";
                case nameof(Name): return "ФИО сотрудника";
                case nameof(Login): return "Логин";
                default: return propertyName;
            }
        }

        public override bool IsReadyToInsert()
        {
            if (Role == Roles.None) return false;
            if(Role == Roles.Supervisor)
                return base.IsReadyToInsert() &&
                       Login.Length > 3;
            return base.IsReadyToInsert() &&
                   Login.Length > 3 &&
                   !string.IsNullOrWhiteSpace(Agency);
        }

        public override string GetParameterList()
        {
            return base.GetParameterList() + $", '{Login}'" +
                   (Role == Roles.Employee ? $", '{Agency}'" : "");
        }

        public override string GetIdentifiedParameterList()
        {
            return base.GetIdentifiedParameterList() + $", '{Login}'" +
                   (Role == Roles.Employee ? $", '{Agency}'" : "");
        }

        public override bool Equals(ITableEntry record)
        {
            if (!base.Equals(record))
                return false;

            if (!(record is User user)) return false;

            return string.Compare(Login, user.Login,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(Agency, user.Agency,
                       StringComparison.CurrentCulture) == 0;
        }

        public override string ToString()
        {
            return base.ToString() + $", Login=\"{Login}\", " +
                   $"Role=\"{Role}\", Agency=\"{Agency}\", ";
        }
    }
}