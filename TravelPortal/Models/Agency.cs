using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Agency : SimpleRecord
    {
        private string _registration;
        private string _city;
        private string _address;
        private string _ownership;
        private string _phone;
        private DateTime _date;

        public string Registration
        {
            get => _registration;
            set
            {
                _registration = value;
                OnPropertyChanged(nameof(Registration));
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

        public string Address {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        public string Ownership {
            get => _ownership;
            set
            {
                _ownership = value;
                OnPropertyChanged(nameof(Ownership));
            }
        }

        public string Phone {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        public DateTime Date {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        public new static readonly Agency Empty;

        static Agency()
        {
            Empty = new Agency(-1, "", "", "", "", "", "", DateTime.Today);
        }
        
        public Agency(int id, string registration, string name, string city,
            string address, string ownership, string phone, DateTime date) : base(id, name)
        {
            Registration = registration;
            City = city;
            Address = address;
            Ownership = ownership;
            Phone = phone;
            Date = date;
        }

        public Agency(Agency newRecord) : base(newRecord.Id, newRecord.Name)
        {
            Registration = newRecord.Registration;
            City = newRecord.City;
            Address = newRecord.Address;
            Ownership = newRecord.Ownership;
            Phone = newRecord.Phone;
            Date = newRecord.Date;
        }

        /// <summary>
        /// Возвращает подпись для столбца таблицы по имени его свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства объекта коллекции в таблице.</param>
        /// <returns></returns>
        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return "Агенство";
                case nameof(Registration): return "Регистрация";
                case nameof(City): return "Город";
                case nameof(Address): return "Адрес";
                case nameof(Ownership): return "Собственность";
                case nameof(Phone): return "Телефон";
                case nameof(Date): return "Дата основания";
                default: return propertyName;
            }
        }

        public static List<string> GenerateAllTitles()
        {
            List<string> titles = new List<string>
            {
                "Агенство",
                "Регистрация",
                "Город",
                "Адрес",
                "Собственность",
                "Телефон",
                "Дата основания"
            };
            return titles;
        }

        public override bool IsReadyToInsert()
        {
            return base.IsReadyToInsert() && !string.IsNullOrWhiteSpace(Registration) &&
                   !string.IsNullOrWhiteSpace(City) && !string.IsNullOrWhiteSpace(Address) &&
                   !string.IsNullOrWhiteSpace(Ownership) && !string.IsNullOrWhiteSpace(Phone) &&
                   Date <= DateTime.Today;
        }

        public override string GetParameterList()
        {
            return base.GetParameterList() + $", '{Registration}', '{City}', " +
                   $"'{Address}', '{Ownership}', '{Phone}', '{new NpgsqlDate(Date)}'";
        }

        public override string GetIdentifiedParameterList()
        {
            return $"{Id}, {GetParameterList()}";
        }

        public override bool Equals(SimpleRecord record)
        {
            if (!base.Equals(record))
                return false;

            if (!(record is Agency agency)) return false;

            return string.Compare(Registration, agency.Registration,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(City, agency.City,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(Address, agency.Address,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(Ownership, agency.Ownership,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(Phone, agency.Phone,
                       StringComparison.CurrentCulture) == 0 &&
                   Date.Equals(agency.Date);
        }

        public override int GetHashCode()
        {
            return (Name + Registration + City + Address,
                Ownership + Phone + Date).GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + $", Registration=\"{Registration}\", " +
                   $"City=\"{City}\", Address=\"{Address}\", " +
                   $"Ownership=\"{Ownership}\", Phone=\"{Phone}\", Date={Date.ToShortDateString()}";
        }
    }
}