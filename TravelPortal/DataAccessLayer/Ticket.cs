﻿using System;

namespace TravelPortal.DataAccessLayer
{
    public class Ticket : SimpleRecord
    {
        private string _from;
        private string _to;
        private double _cost;

        public string From
        {
            get => _from;
            set
            {
                _from = value;
                OnPropertyChanged(nameof(From));
            }
        }
        
        public string To
        {
            get => _to;
            set
            {
                _to = value;
                OnPropertyChanged(nameof(To));
            }
        }

        public double Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                OnPropertyChanged(nameof(From));
            }
        }

        public new static readonly Ticket Empty;

        static Ticket() { Empty = new Ticket(-1, "", "", "", 0); }

        public Ticket(int id, string transport, string from, string to,
            double cost) : base(id, transport)
        {
            From = from;
            To = to;
            Cost = cost;
        }

        public Ticket(Ticket newRecord) : base(newRecord.Id, newRecord.Name)
        {
            From = newRecord.From;
            To = newRecord.To;
            Cost = newRecord.Cost;
        }

        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return "Транспорт";
                case nameof(From): return "Откуда";
                case nameof(To): return "Куда";
                case nameof(Cost): return "Стоимость";
                default: return propertyName;
            }
        }

        public override bool IsReadyToInsert()
        {
            return base.IsReadyToInsert() && !string.IsNullOrWhiteSpace(From) &&
                   !string.IsNullOrWhiteSpace(To) &&
                   Cost > 0;
        }

        public override string GetParameterList()
        {
            return base.GetParameterList() + $", '{From}', '{To}', {Cost}";
        }

        public override string GetIdentifiedParameterList()
        {
            return $"{Id}, {GetParameterList()}";
        }

        public override bool Equals(ITableEntry record)
        {
            if (!base.Equals(record))
                return false;

            if (!(record is Ticket hotel)) return false;

            return string.Compare(From, hotel.From,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(To, hotel.To,
                       StringComparison.CurrentCulture) == 0 &&
                   Math.Abs(Cost - hotel.Cost) < 0.01;
        }

        public override int GetHashCode()
        {
            return (Name + From + To + Cost).GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", From=\"{From}\", To=\"{To}\", Cost={Cost}";
        }
    }
}