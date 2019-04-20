namespace TravelPortal.Models
{
    public class Ticket
    {
        private int _ticketId;
        public string From { get; set; }
        public string To { get; set; }
        public string Transport { get; set; }
        public double Cost { get; set; }

        public Ticket(int ticketId, string @from, string to, string transport,
            double cost)
        {
            _ticketId = ticketId;
            From = @from;
            To = to;
            Transport = transport;
            Cost = cost;
        }
    }
}