using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    public static class Queries
    {
        public static class Routes
        {
            public const string selectAllFunction =
                "select * from select_all_from_routes()";

            public const string SelectAll =
                "select route_id, (select name from city where city_id = routes.from_id), " +
                "(select name from city where city_id = routes.to_id), cost, date, duration, " +
                "(select name from hotel where hotel_id = routes.hotel_id), meels, " +
                "(select name from transport where transport_id = (select transport_id from tickets where ticket_id = routes.ticket_id)), " +
                "(select cost from tickets where ticket_id = routes.ticket_id) " +
                "from routes ";

            public static string Search(NpgsqlDate date) =>
                SelectAll + $"where date = '{date}'";

            public static string Search(int duration) =>
                SelectAll + $"where duration = '{duration}'";

            public static string FilterHotel(string hotel) =>
                SelectAll + $"where hotel_id = (select hotel_id from hotel where lower(name) like lower('{hotel}'))";

            public static string FilterTransport(string transport) =>
                SelectAll + $"where lower('{transport}') like " +
                "lower((select name from transport where transport_id = (select transport_id from tickets where ticket_id= routes.ticket_id)))";
        }

        public static class Vouchers
        {
            public const string SelectAll =
                "select voucher_id, customer_fio, " +
                "(select name from city where city_id = (select from_id from routes where route_id = vouchers.route_id)), " +
                "(select name from city where city_id = (select to_id from routes where route_id = vouchers.route_id)), " +
                "(select name from hotel where hotel_id = (select hotel_id from routes where route_id=vouchers.route_id))," +
                "phone, address, birthday from vouchers ";

            public static string Search(string fio) =>
                SelectAll + $"where lower(customer_fio) like lower('%{fio}%')";

            public static string FilterStatus(string status)=>
                SelectAll + $"where status_id = (select status_id from status where lower(name) like lower('{status}'))";
        }
        
        public static string SelectAllTransport = "select name from transport ";

        public static string SelectAllStatus = "select name from status ";

        public static class Dictionaries
        {
            public const string SelectAllAgencies =
                "select agency_id, regnumber, name, " +
                "(select name from city where city_id = agencies.city_id), address, " +
                "(select name from ownership where ownership_id = agencies.ownership_id), " +
                "phone, date from agencies";
            
            public static string SelectAllTickets =
                "select ticket_id, " +
                "(select name from city where city_id = tickets.from_id), " +
                "(select name from city where city_id = tickets.to_id), " +
                "(select name from transport where transport_id = tickets.transport_id), " +
                "cost from tickets ";

            public static string SelectNameList(DictionaryKind dictionary) =>
                $"select * from select_name_from_{dictionary}()";

            public static string SelectAll(DictionaryKind dictionary) =>
                $"select * from select_all_from_{dictionary}()";

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