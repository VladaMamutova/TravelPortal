using NpgsqlTypes;

namespace TravelPortal.Database
{
    public static class Queries
    {
        public static class Routes
        {
            public const string SelectAll =
                "select route_id, name, cost, date, duration from routes ";

            public static string FilterName(string name) =>
                SelectAll + $"where lower(name) like lower('%{name}%')";
            
            public static string FilterDate(NpgsqlDate date) =>
                SelectAll + $"where date = '{date}'";

            public static string FilterDuration(int duration) =>
                SelectAll + $"where duration = '{duration}'";
        }

        public static class Vouchers
        {
            public const string SelectAll =
                "select voucher_id, customer_fio, phone, address, birthday from vouchers ";

            public static string FilterFio(string fio) =>
                SelectAll + $"where lower(customer_fio) like lower('%{fio}%')";

            public static string FilterStatus(string status)=>
                SelectAll + $"where status_id = (select status_id from status where lower(type) like lower('{status}'))";
        }
    }
}