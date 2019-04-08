using System.Collections.Generic;
using Npgsql;
using TravelPortal.Database;

namespace TravelPortal.ViewModels
{
    class RouteViewModel
    {
        public List<string> HotelCollection { get; }
        public string SelectedHotel { get; set; }

        public RouteViewModel()
        {
            using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();
                using (var command =
                    new NpgsqlCommand(Queries.SelectAllHotels, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.HasRows) HotelCollection = null;
                        List<string> hotels = new List<string>();
                        while (reader.Read())
                            hotels.Add(reader.GetString(0).TrimEnd(' '));

                        HotelCollection = new List<string>(hotels);
                    }
                }
            }
        }
    }
}
