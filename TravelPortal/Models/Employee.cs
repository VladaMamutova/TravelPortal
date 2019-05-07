namespace TravelPortal.Models
{
    public class Employee
    {
        private int _id;
        public string Agency { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }

        public Employee(int id, string agency, string city, string name,
            string login)
        {
            _id = id;
            Agency = agency;
            City = city;
            Name = name;
            Login = login;
        }
    }
}