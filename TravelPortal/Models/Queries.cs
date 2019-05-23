using System;
using NpgsqlTypes;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    public static class Queries
    {
        private static Roles _role;
        private static int _agencyId;

        static Queries()
        {
            _role = Roles.None;
            _agencyId = -1;
        }

        public static void SetRole(Roles role, int agencyId)
        {
            _role = role;
            _agencyId = agencyId;
        }

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

            public static string Delete(DictionaryKind dictionary,
                SimpleRecord record)
            {
                return $"select delete_{dictionary}" +
                       $"({record.GetId()})";
            }
        }

        public static string SelectUser(string login)
        {
            return $"select * from select_user('{login}')";
        }

        public static string GetRoutes()
        {
            if (_role == Roles.Admin)
                return "select * from route_view";
            return $"select * from get_routes_from_agency({_agencyId})";
        }

        public static string GetVouchers()
        {
            if (_role == Roles.Admin)
                return "select * from voucher_view";
            return $"select * from get_vouchers_from_agency({_agencyId})";
        }

        public static string GetCustomers()
        {
            if (_role == Roles.Admin)
                return "select * from customer_view";
            return $"select * from get_customers_from_agency({_agencyId})";
        }
        
        public static string SelectAgenciesWithStaff => "select * from get_agencies_with_staff()"; // user_view

        public static string RankByPopularity =>
            "select * from rank_agencies_by_popularity()"; 
        public static string RankByNumberOfRoutes =>
            "select * from rank_agencies_by_the_number_of_routes()";

        public static string RankByGrossProfit(string ownership, string agency)
        {
            //  В зависимости от передаваемых параметров, вызываем запрос на нужный фильтр.

            if(string.IsNullOrEmpty(agency) && string.IsNullOrEmpty(ownership))
                return "select * from rank_agencies_by_gross_profit()";

            if (!string.IsNullOrEmpty(agency) && !string.IsNullOrEmpty(ownership))
                return "select * from rank_agencies_by_gross_profit_with_ownership_begins" +
                       $"_with('{ownership}', '{agency}')";

            if (!string.IsNullOrEmpty(agency))
                return $"select * from rank_agencies_by_gross_profit_begins_with('{agency}')";

            return $"select * from rank_agencies_by_gross_profit_with_ownership('{ownership}')";
        }

        public static string GetUser(string login) => $"select * from get_user('{login}')";

        public static string InsertVoucher(int routeId, Customer customer)
        {
            return $"select insert_voucher({routeId}, {customer.GetParameterList()})";
        }

        public static string RankHotels() => "select * from rank_hotels()";

        public static string AddUser(User user, string password)
        {
            switch (user.Role)
            {
                case Roles.Employee:
                    return $"select add_employee({user.GetParameterList()}, '{password}')";
                case Roles.Supervisor:
                    return $"select add_supervisor({user.GetParameterList()}, '{password}')";
                default: throw new ArgumentException(nameof(user.Role));
            }
        }

        public static string DeleteUser(int userId) =>
            $"select delete_user({userId})";
    }
}