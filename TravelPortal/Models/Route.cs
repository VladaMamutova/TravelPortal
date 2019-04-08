using NpgsqlTypes;

namespace TravelPortal.Models
{
    public class Route
    {
        private int _routeId;
        //private int _residenceId;
        //private int _transportTypeId;
        //private int _agencyId;

        public string Name { get; set; }
        public NpgsqlDate Date { get; set; }
        //public bool Meels { get; set; }
        public int Duration { get; set; }
        public double Cost { get; set; }
        //public string Transport { get; set; }
        //public string Residence { get; set; }

        public Route(int routeId, string name, NpgsqlDate date, int duration, double cost)
        {
            _routeId = routeId;
            Name = name;
            Date = date;
            Duration = duration;
            Cost = cost;
        }
    }
}