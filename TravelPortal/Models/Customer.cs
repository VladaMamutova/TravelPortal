using System;

namespace TravelPortal.Models
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
    }
}