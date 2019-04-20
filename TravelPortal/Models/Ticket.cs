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

        public static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(From): return "Откуда";
                case nameof(To): return "Куда";
                case nameof(Transport): return "Вид транспорта";
                case nameof(Cost): return "Стоимость";
                default: return propertyName;
            }
        }
    }
}