using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Agency
    {
        private int _id;
        public string Registration { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Ownership { get; set; }
        public string Phone { get; set; }
        public NpgsqlDate Date { get; set; }

        public int GetId() => _id;

        public Agency(int id, string registration, string name, string city,
            string address, string ownership, string phone, NpgsqlDate date)
        {
            _id = id;
            Registration = registration;
            Name = name;
            City = city;
            Address = address;
            Ownership = ownership;
            Phone = phone;
            Date = date;
        }

        /// <summary>
        /// Возвращает подпись для столбца таблицы по имени его свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства объекта коллекции в таблице.</param>
        /// <returns></returns>
        public static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Registration): return "Регистрационный номер";
                case nameof(Name): return "Название";
                case nameof(City): return "Город";
                case nameof(Address): return "Адрес";
                case nameof(Ownership): return "Тип собственности";
                case nameof(Phone): return "Телефон";
                case nameof(Date): return "Дата основания";
                default: return propertyName;
            }
        }
    }
}