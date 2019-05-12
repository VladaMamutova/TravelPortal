using System;

namespace TravelPortal.DataAccessLayer
{
    public class Hotel : SimpleRecord
    {
        private string _city;
        private int _type;

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        public int Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
            }
        }

        public new static readonly Hotel Empty;

        static Hotel() { Empty = new Hotel(-1, "", "", 0); }

        public Hotel(Hotel newRecord) : base(newRecord.GetId(), newRecord.Name)
        {
            City = newRecord.City;
            Type = newRecord.Type;
        }

        public Hotel(int id, string name, string city, int type) : base(id, name)
        {
            City = city;
            Type = type;
        }

        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return "Отель";
                case nameof(City): return "Город";
                case nameof(Type): return "Категория";
                default: return propertyName;
            }
        }

        public override bool IsReadyToInsert()
        {
            return base.IsReadyToInsert() && !string.IsNullOrWhiteSpace(City) &&
                   Type > 0;
        }

        public override string GetParameterList()
        {
            return base.GetParameterList() + $", '{City}', {Type}";
        }

        public override string GetIdentifiedParameterList()
        {
            return $"{Id}, {GetParameterList()}";
        }

        public override bool Equals(SimpleRecord record)
        {
            if (!base.Equals(record))
                return false;

            if (!(record is Hotel hotel)) return false;

            return string.Compare(City, hotel.City,
                       StringComparison.CurrentCulture) == 0 &&
                   Type == hotel.Type;
        }

        public override int GetHashCode()
        {
            return (Name + City + Type).GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() + $", City=\"{City}\", Type=\"{Type}\"";
        }
    }
}