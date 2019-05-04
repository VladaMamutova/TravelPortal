using System.Collections.Generic;
using Npgsql;
using TravelPortal.Database;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class VoucherViewModel
    {
        public List<string> StatusCollection { get; }
        public string SelectedStatus { get; set; }

        public VoucherViewModel()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();

                StatusCollection =
                    GetCollection(connection, Queries.Dictionaries.SelectNameList(DictionaryKind.Status));
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