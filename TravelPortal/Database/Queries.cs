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
        }

        public static class Vouchers
        {
            public const string SelectAll =
                "select voucher_id, client_fio, phone, address, birthday from vouchers ";

            public static string FilterFio(string fio) =>
                SelectAll + $"where lower(client_fio) like lower('%{fio}%')";
        }
    }
}