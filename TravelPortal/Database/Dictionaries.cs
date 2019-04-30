using System.Collections.ObjectModel;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Models;

namespace TravelPortal.Database
{
    static class Dictionaries
    {
        public static ObservableCollection<SimpleRecord> GetDictionary(
            DictionaryKind dictionary)
        {
            switch (dictionary)
            {
                case DictionaryKind.Hotel: return GetHotels();
                default: return GetSimpleDictionary(dictionary);
            }
        }

        private static ObservableCollection<SimpleRecord> GetSimpleDictionary(
            DictionaryKind dictionary)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.SelectAll(dictionary), connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<SimpleRecord> collection =
                            new ObservableCollection<SimpleRecord>();
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string name = reader.GetString(1).TrimEnd();

                            collection.Add(new SimpleRecord(id, name));
                        }

                        return collection;
                    }
                }
            }
        }

        private static ObservableCollection<Agency> GetAgencies(
            NpgsqlConnection npgsqlConnection)
        {
            using (var command = new NpgsqlCommand(
                Queries.Dictionaries.SelectAllAgencies, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<Agency> collection =
                        new ObservableCollection<Agency>();
                    while (reader.Read())
                    {
                        int agencyId = reader.GetInt32(0);
                        string registration = reader.GetString(1).TrimEnd();
                        string name = reader.GetString(2).TrimEnd();
                        string city = reader.GetString(3).TrimEnd();
                        string address = reader.GetString(4).TrimEnd();
                        string ownership = reader.GetString(5).TrimEnd();
                        string phone = reader.GetString(6).TrimEnd();
                        NpgsqlDate date = reader.GetDate(7);

                        collection.Add(new Agency(agencyId, registration, name,
                            city, address, ownership, phone, date));
                    }

                    return collection;
                }
            }
        }

        private static ObservableCollection<SimpleRecord> GetHotels()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.SelectAll(DictionaryKind.Hotel), connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<SimpleRecord> collection =
                            new ObservableCollection<SimpleRecord>();
                        while (reader.Read())
                        {
                            int hotelId = reader.GetInt32(0);
                            string name = reader.GetString(1).TrimEnd();
                            int type = reader.GetInt32(2);

                            collection.Add(new Hotel(hotelId, name, type));
                        }

                        return collection;
                    }
                }
            }
        }

        private static ObservableCollection<Ticket> GetTickets(
            NpgsqlConnection npgsqlConnection)
        {
            using (var command = new NpgsqlCommand(
                Queries.Dictionaries.SelectAllTickets, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<Ticket> collection =
                        new ObservableCollection<Ticket>();
                    while (reader.Read())
                    {
                        int ticketId = reader.GetInt32(0);
                        string from = reader.GetString(1).TrimEnd();
                        string to = reader.GetString(2).TrimEnd();
                        string transport = reader.GetString(3).TrimEnd();
                        double cost = reader.GetDataTypeOID(4);

                        collection.Add(new Ticket(ticketId, from, to, transport,
                            cost));
                    }

                    return collection;
                }
            }
        }
    }
}
