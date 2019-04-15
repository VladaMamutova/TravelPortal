using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Voucher
    {
        private int _voucherId;

        public string ClientFio { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Hotel { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public NpgsqlDate Birthday { get; set; }

        public Voucher(int voucherId, string clientFio, string from, string to,
            string hotel, string address, string phone, NpgsqlDate birthday)
        {
            _voucherId = voucherId;
            ClientFio = clientFio;
            From = from;
            To = to;
            Hotel = hotel;
            Address = address;
            Phone = phone;
            Birthday = birthday;
        }
    }
}
