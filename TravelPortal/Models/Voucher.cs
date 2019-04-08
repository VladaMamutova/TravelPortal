using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Voucher
    {
        private int _voucherId;
        //private int _routeId;
        //private int _status_id;

        public string ClientFio { get; set; }
        public NpgsqlDate Birthday { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Voucher(int voucherId, string clientFio, NpgsqlDate birthday, string address, string phone)
        {
            _voucherId = voucherId;
            ClientFio = clientFio;
            Birthday = birthday;
            Address = address;
            Phone = phone;
        }
    }
}
