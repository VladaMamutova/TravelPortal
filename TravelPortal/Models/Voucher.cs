using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Voucher
    {
        private int _voucherId;

        public string ClientFio { get; set; }
        public string Route { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public NpgsqlDate Birthday { get; set; }

        public Voucher(int voucherId, string clientFio, string route,
            string address, string phone, NpgsqlDate birthday)
        {
            _voucherId = voucherId;
            ClientFio = clientFio;
            Route = route;
            Address = address;
            Phone = phone;
            Birthday = birthday;
        }
    }
}
