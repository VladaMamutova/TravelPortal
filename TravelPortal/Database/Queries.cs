using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public static class Queries
    {
        public static string SelectCustomerView => "select * from customer_view";
        public static string SelectRouteView => "select * from route_view";
        public static string SelectVoucherView => "select * from voucher_view";
        public static string SelectEmployeeView => "select * from employee_view";

        public static string SelectHotelNameView => "select * from hotel_name_view";
        public static string SelectStatusNameView => "select * from status_name_view";
        public static string SelectOwnershipNameView => "select * from ownership_name_view";
        public static string SelectCityNameView => "select * from city_name_view";
        public static string SelectTransportNameView => "select * from transport_name_view";

        public static class Routes
        {
            public const string SelectAllFunction =
                "select * from select_all_from_route()";


            public static string Search(NpgsqlDate date) =>
                SelectAllFunction + $"where date = '{date}'";

            public static string Search(int duration) =>
                SelectAllFunction + $"where duration = '{duration}'";

            public static string FilterHotel(string hotel) =>
                SelectAllFunction + $"where hotel_id = (select hotel_id from hotel where lower(name) like lower('{hotel}'))";

            public static string FilterTransport(string transport) =>
                SelectAllFunction + $"where lower('{transport}') like " +
                "lower((select name from transport where transport_id = (select transport_id from ticket where ticket_id = route.ticket_id)))";
        }

        public static class Vouchers
        {
            public const string SelectAllFunction =
                "select * from select_all_from_voucher()";

            public static string Search(string fio) =>
                SelectAllFunction + $"where lower(fio) like lower('%{fio}%')";

            public static string FilterStatus(string status)=>
                SelectAllFunction + $"where lower(status) like lower('{status}'))";
        }

        public static class Dictionaries
        {
            public static string SelectNameView(DictionaryKind dictionary) =>
                $"select * from {dictionary}_name_view";

            public static string SelectAll(DictionaryKind dictionary) =>
                $"select * from {dictionary}_view";

            public static string Insert(DictionaryKind dictionary,
                SimpleRecord record)
            {
                return $"select insert_{dictionary}" +
                       $"({record.GetParameterList()})";
            }

            public static string Update(DictionaryKind dictionary,
                SimpleRecord record)
            {
                return $"select update_{dictionary}" +
                       $"({record.GetIdentifiedParameterList()})";
            }
        }
    }
}