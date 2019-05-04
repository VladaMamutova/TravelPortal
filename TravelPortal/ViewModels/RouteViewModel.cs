using System.Collections.Generic;
using Npgsql;
using TravelPortal.Database;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для страницы маршрутов.
    /// </summary>
    class RouteViewModel
    {
        public List<string> HotelCollection { get; }
        public string SelectedHotel { get; set; }

        public List<string> TransportCollection { get; }
        public string SelectedTransport { get; set; }

        public RouteViewModel()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();

                HotelCollection =
                    GetCollection(connection, Queries.Dictionaries.SelectNameList(DictionaryKind.Hotel));
                TransportCollection = GetCollection(connection,
                    Queries.Dictionaries.SelectNameList(DictionaryKind.Transport));
            }
        }

        private List<string> GetCollection(NpgsqlConnection npgsqlConnection, string query)
        {
            using (var command = new NpgsqlCommand(query, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    List<string> collection = new List<string>();
                    while (reader.Read())
                        collection.Add(reader.GetString(0).TrimEnd(' '));

                    return collection;
                }
            }
        }
    }
}
