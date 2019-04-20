namespace TravelPortal.Models
{
    public class Ticket
    {
        private int _ticketId;
        public string From { get; set; }
        public string To { get; set; }
        public string Transport { get; set; }
        public double Cost { get; set; }
    }
}