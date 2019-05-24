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

        public static class MainTables
        {
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
           
            public static string FilterRoutes(Route example) =>
                $"select * from filter_route({_agencyId}, {example.GetParameterListForFilter()})";

            public static string FilterCustomers(Customer customer) =>
                $"select * from filter_customer({_agencyId}, '{customer.Fio}', '{customer.Phone}')";
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

        public static string UpdateUser(User user, string password)
        {
            switch (user.Role)
            {
                case Roles.Employee:
                    return $"select update_employee({user.GetIdentifiedParameterList()}, '{password}')";
                case Roles.Supervisor:
                    return $"select update_supervisor({user.GetIdentifiedParameterList()}, '{password}')";
                default: throw new ArgumentException(nameof(user.Role));
            }
        }

        public static string DeleteUser(int userId) =>
            $"select delete_user({userId})";
    }
}