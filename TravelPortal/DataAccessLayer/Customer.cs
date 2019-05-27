using System;
using NpgsqlTypes;

namespace TravelPortal.DataAccessLayer
{
    public class Customer : SimpleRecord
    {
        public int VoucherCount { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string Status { get; set; }

        public new static readonly Customer Empty;

        static Customer()
        {
            Empty = new Customer(-1, "", "", "",
                new DateTime(DateTime.Today.Year - 16,
                    DateTime.Today.Month, DateTime.Today.Day), "");
        }

        public Customer(int voucherCount, string name, string phone,
            string address, DateTime birthday, string status) : base(-1, name)
        {
            VoucherCount = voucherCount;
            Phone = phone;
            Address = address;
            Birthday = birthday;
            Status = status;
        }

        public Customer(Customer customer) : base(-1, customer.Name)
        {
            VoucherCount = customer.VoucherCount;
            Phone = customer.Phone;
            Address = customer.Address;
            Birthday = customer.Birthday;
            Status = customer.Status;
        }

        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(VoucherCount): return "Всего путёвок";
                case nameof(Name): return "ФИО клиента";
                case nameof(Phone): return "Телефон";
                case nameof(Address): return "Адрес";
                case nameof(Birthday): return "Дата рождения";
                case nameof(Status): return "Социальное положение";
                default: return propertyName;
            }
        }

        public new bool IsReadyToInsert()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(Address) && !string.IsNullOrWhiteSpace(Status) &&
                   Birthday.Year <= DateTime.Today.Year - 16;
        }

        public new string GetParameterList()
        {
            return $"'{Name}', '{Phone}', '{Address}', '{Status}', '{new NpgsqlDate(Birthday)}'";
        }

        public override string GetIdentifiedParameterList()
        {
            return $"'{Name}', '{Phone}', '{Address}', '{Status}'";
        }

        public override bool Equals(ITableEntry record)
        {
            if (!base.Equals(record))
                return false;

            if (!(record is Customer route)) return false;

            return string.Compare(Phone, route.Phone,
                       StringComparison.CurrentCulture) == 0 &&
                   string.Compare(Address, route.Address,
                       StringComparison.CurrentCulture) == 0 &&
                   Birthday == route.Birthday &&
                   VoucherCount == route.VoucherCount &&
                   string.Compare(Status, route.Status,
                       StringComparison.CurrentCulture) == 0;
        }

        public override string ToString()
        {
            return $"Name=\"{Name}\", Phone=\"{Phone}\", " +
                   $"Address=\"{Address}\", Status=\"{Status}\", " +
                   $"Date=\"{Birthday}\"";
        }
    }
}