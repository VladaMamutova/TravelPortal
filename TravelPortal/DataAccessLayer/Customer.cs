using System;
using NpgsqlTypes;

namespace TravelPortal.DataAccessLayer
{
    public class Customer
    {
        public int VoucherCount { get; set; }
        public string Fio { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string Status { get; set; }

        public Customer() {  Birthday = new DateTime(DateTime.Today.Year - 16, DateTime.Today.Month, DateTime.Today.Day);}

        public Customer(int voucherCount, string fio, string phone, string address, DateTime birthday, string status)
        {
            VoucherCount = voucherCount;
            Fio = fio;
            Phone = phone;
            Address = address;
            Birthday = birthday;
            Status = status;
        }

        public static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(VoucherCount): return "Количество путёвок";
                case nameof(Fio): return "ФИО клиента";
                case nameof(Phone): return "Телефон";
                case nameof(Address): return "Адрес";
                case nameof(Birthday): return "Дата рождения";
                case nameof(Status): return "Социальное положение";
                default: return propertyName;
            }
        }

        public bool IsReadyToInsert()
        {
            return !string.IsNullOrWhiteSpace(Fio) && !string.IsNullOrWhiteSpace(Phone) &&
                   !string.IsNullOrWhiteSpace(Address) && !string.IsNullOrWhiteSpace(Status) &&
                   Birthday.Year <= DateTime.Today.Year - 16;
        }

        public string GetParameterList()
        {
            return $"'{Fio}', '{Phone}', '{Address}', '{Status}', '{new NpgsqlDate(Birthday)}'";
        }
    }
}