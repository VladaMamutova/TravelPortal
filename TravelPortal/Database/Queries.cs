using NpgsqlTypes;

namespace TravelPortal.Database
{
    public static class Queries
    {
        public static class Routes
        {
            public const string SelectAll =
                "select route_id, name, cost, date, duration, " +
                "(select name from residence where residence_id = routes.residence_id), meels, " +
                "(select name from transport_type where transport_type_id = routes.transport_type_id) " +
                //", (select cost from transport_type where transport_type_id = routes.transport_type_id) " +
                "from routes ";

            public static string FilterName(string name) =>
                SelectAll + $"where lower(name) like lower('%{name}%')";
            
            public static string FilterDate(NpgsqlDate date) =>
                SelectAll + $"where date = '{date}'";

            public static string FilterDuration(int duration) =>
                SelectAll + $"where duration = '{duration}'";

            public static string FilterResidence(string residence) =>
                SelectAll + $"where residence_id = (select residence_id from residence where lower(name) like lower('{residence}'))";

            public static string FilterTransport(string transport) =>
                SelectAll + $"where transport_type_id = (select transport_type_id from transport_type where lower(name) like lower('{transport}'))";
        }

        public static class Vouchers
        {
            public const string SelectAll =
                "select voucher_id, customer_fio, " +
                "(SELECT name FROM routes where route_id=vouchers.route_id), " +
                "phone, address, birthday from vouchers ";

            public static string FilterFio(string fio) =>
                SelectAll + $"where lower(customer_fio) like lower('%{fio}%')";

            public static string FilterStatus(string status)=>
                SelectAll + $"where status_id = (select status_id from status where lower(type) like lower('{status}'))";
        }

        public static string SelectAllHotels = "select name from residence ";

        public static string SelectAllTransport = "select name from transport_type ";

        public static string SelectAllStatus = "select type from status ";
    }
}