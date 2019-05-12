using System;

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
    }
}