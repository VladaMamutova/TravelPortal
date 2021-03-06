﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.Models
{
    static class Dictionaries
    {
        private static readonly Configuration Configuration;

        static Dictionaries()
        {
            Configuration = Configuration.GetConfiguration();
        }

        public static ObservableCollection<SimpleRecord> GetDictionary(
            DictionaryKind dictionary)
        {
            switch (dictionary)
            {
                case DictionaryKind.Hotel: return GetHotels();
                case DictionaryKind.Ticket: return GetTickets();
                case DictionaryKind.Agency: return GetAgencies();
                default: return GetSimpleDictionary(dictionary);
            }
        }
        
        public static List<string> GetNameList(
            DictionaryKind dictionary)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.SelectNameView(dictionary),
                    connection))
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

        public static object ExecuteQuery(string query)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                using (var command =
                    new NpgsqlCommand(query, connection))
                {
                    return command.ExecuteScalar();
                }
            }
        }

        private static ObservableCollection<SimpleRecord> GetSimpleDictionary(
            DictionaryKind dictionary)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
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

        private static ObservableCollection<SimpleRecord> GetAgencies()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.SelectAll(DictionaryKind.Agency), connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<SimpleRecord> collection =
                            new ObservableCollection<SimpleRecord>();
                        while (reader.Read())
                        {
                            int agencyId = reader.GetInt32(0);
                            string name = reader.GetString(1).TrimEnd();
                            string registration = reader.GetString(2).TrimEnd();
                            string city = reader.GetString(3).TrimEnd();
                            string address = reader.GetString(4).TrimEnd();
                            string ownership = reader.GetString(5).TrimEnd();
                            string phone = reader.GetString(6).TrimEnd();
                            NpgsqlDate date = reader.GetDate(7);

                            collection.Add(new Agency(agencyId, registration,
                                name,
                                city, address, ownership, phone, new DateTime(date.Year, date.Month, date.Day)));
                        }

                        return collection;
                    }
                }
            }
        }

        private static ObservableCollection<SimpleRecord> GetHotels()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
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
                            string city = reader.GetString(2).TrimEnd();
                            int type = reader.GetInt32(3);

                            collection.Add(new Hotel(hotelId, name, city, type));
                        }

                        return collection;
                    }
                }
            }
        }

        public static Dictionary<string, KeyValuePair<string, int>> GetHotelCityTypeCollection()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.GetHotelCityTypeCollection, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        Dictionary<string, KeyValuePair<string, int>> collection
                            = new Dictionary<string, KeyValuePair<string, int>>();
                        while (reader.Read())
                        {
                            string hotel = reader.GetString(0);
                            string city = reader.GetString(1);
                            int type = reader.GetInt32(2);
                            
                            collection.Add(hotel, new KeyValuePair<string, int>(city, type));
                        }

                        return collection;
                    }
                }
            }
        }

        public static Dictionary<string, double> GetTransportPriceCollection(string from, string to)
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.GetFromToPossibleTransportCollection(from, to), connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        Dictionary<string, double> collection = new Dictionary<string, double>();

                        while (reader.Read())
                            collection.Add(reader.GetString(0), reader.GetDouble(1));

                        return collection;
                    }
                }
            }
        }

        private static ObservableCollection<SimpleRecord> GetTickets()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand(
                    Queries.Dictionaries.SelectAll(DictionaryKind.Ticket), connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) return null;
                        ObservableCollection<SimpleRecord> collection =
                            new ObservableCollection<SimpleRecord>();
                        while (reader.Read())
                        {
                            int ticketId = reader.GetInt32(0);
                            string name = reader.GetString(1).TrimEnd();
                            string from = reader.GetString(2).TrimEnd();
                            string to = reader.GetString(3).TrimEnd();
                            double cost = reader.GetDouble(4);

                            collection.Add(new Ticket(ticketId, name, from, to,
                                cost));
                        }

                        return collection;
                    }
                }
            }
        }
    }
}
