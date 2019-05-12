using TravelPortal.Models;

namespace TravelPortal.DataAccessLayer
{
    public class User
    {
        private readonly int _id;
        private readonly int _agencyId;
        //private string _password;
        public Roles Role { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Agency { get; set; }
        public string City { get; set; }

        public int GetId() => _id;
        public int GetAgencyId() => _agencyId;
        //public string GetPassword() => _password;

        public User(int id, Roles role, string name, string login, int agencyId, string agency, string city)
        {
            _id = id;
            _agencyId = agencyId;
            Role = role;
            Name = name;
            Login = login;
            Agency = agency;
            City = city;
        }

        public static string GenerateTitle(string propertyName)
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
    }
}