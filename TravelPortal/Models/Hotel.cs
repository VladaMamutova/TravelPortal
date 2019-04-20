namespace TravelPortal.Models
{
    public class Hotel
    {
        private int _hotelId;
        public string Name { get; set; }
        public int Type { get; set; }

        public Hotel(int hotelId, string name, int type)
        {
            _hotelId = hotelId;
            Name = name;
            Type = type;
        }
    }
}