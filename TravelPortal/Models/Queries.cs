using System;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    public static class Queries
    {
        private static int _agencyId;

        static Queries()
        {
            _agencyId = -1;
        }

        public static void SetRole(int agencyId)
        {
            _agencyId = agencyId;
        }

        public static string GetUser(string login) => $"select * from get_user('{login}')";

        public static class MainTables
        {
            public static string InsertRoute(Route route) =>
                $"select insert_route({_agencyId}, {route.GetParameterList()})";

            public static string UpdateRoute(Route route) =>
                $"select update_route({route.GetIdentifiedParameterList()})";
        
            public static string DeleteRoute(int id) =>
                $"select delete_route({id})";

            public static string GetVouchers =>
                $"select * from get_vouchers_from_agency({_agencyId})";

            public static string InsertVoucher(int routeId, Customer customer) =>
                $"select insert_voucher({routeId}, {customer.GetParameterList()})";

            public static string CancelVoucher(int voucherId) =>
                $"select cancel_voucher({voucherId})";

            public static string GetTodayVouchers =>
                $"select * from get_today_vouchers_from_agency({_agencyId})";

            public static string GetTomorrowVouchers =>
                $"select * from get_tomorrow_vouchers_from_agency({_agencyId})";

            public static string GetPastVouchers =>
                $"select * from get_past_vouchers_from_agency({_agencyId})";

            public static string GetFutureVouchers =>
                $"select * from get_future_vouchers_from_agency({_agencyId})";

            public static string FilterRoutes(Route example) =>
                $"select * from filter_route({_agencyId}, {example.GetParameterListForFilter()})";

            public static string FilterCustomers(Customer customer) =>
                $"select * from filter_customer({_agencyId}, '{customer.Name}', '{customer.Phone}')";

            public static string GetCustomers =>
                $"select * from get_customers_from_agency({_agencyId})";
        
            public static string UpdateCustomer(Customer customer) =>
                $"select update_customer({_agencyId}, {customer.GetIdentifiedParameterList()})";

        }

        public static class Dictionaries
        {
            public static string SelectNameView(DictionaryKind dictionary) =>
                $"select * from {dictionary}_name_view";

            public static string SelectAll(DictionaryKind dictionary) =>
                $"select * from {dictionary}_view";

            public static string Insert(DictionaryKind dictionary,
                SimpleRecord record) =>
                $"select insert_{dictionary}({record.GetParameterList()})";

            public static string Update(DictionaryKind dictionary,
                SimpleRecord record) =>
                $"select update_{dictionary}" +
                $"({record.GetIdentifiedParameterList()})";

            public static string Delete(DictionaryKind dictionary,
                SimpleRecord record) =>
                $"select delete_{dictionary}({record.GetId()})";

            public static string GetHotelCityTypeCollection =>
                "select * from get_hotel_city_type_collection()";

            public static string GetFromToPossibleTransportCollection(string cityFrom, string cityTo) =>
                $"select * from get_from_to_possible_transport_with_price('{cityFrom}', '{cityTo}')";
        }

        public static class Ratings
        {
            public static string RankHotels() => "select * from rank_hotels()";

            public static string RankByPopularity =>
                "select * from rank_agencies_by_popularity()";
            public static string RankByNumberOfRoutes =>
                "select * from rank_agencies_by_the_number_of_routes()";

            public static string RankByGrossProfit(string ownership, string agency)
            {
                //  В зависимости от передаваемых параметров, вызываем запрос на нужный фильтр.

                if (string.IsNullOrEmpty(agency) && string.IsNullOrEmpty(ownership))
                    return "select * from rank_agencies_by_gross_profit()";

                if (!string.IsNullOrEmpty(agency) && !string.IsNullOrEmpty(ownership))
                    return "select * from rank_agencies_by_gross_profit_with_ownership_begins" +
                           $"_with('{ownership}', '{agency}')";

                if (!string.IsNullOrEmpty(agency))
                    return $"select * from rank_agencies_by_gross_profit_begins_with('{agency}')";

                return $"select * from rank_agencies_by_gross_profit_with_ownership('{ownership}')";
            }
        }

        public static class Users
        {
            public static string AddUser(User user, string password) =>
                $"select add_user({user.GetParameterList()}, '{password}')";

            public static string UpdateUser(User user, string password) =>
                $"select update_user({user.GetIdentifiedParameterList()}, " +
                $"'{password}')";

            public static string DeleteUser(int userId) =>
                $"select delete_user({userId})";
        }
    }
}