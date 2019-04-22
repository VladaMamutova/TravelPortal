namespace TravelPortal.Models
{
    public class SimpleRecord
    {
        private int _id;
        public string Name { get; set; }

        public SimpleRecord(int id, string name)
        {
            _id = id;
            Name = name;
        }

        public static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return "Наименование";
                default: return propertyName;
            }
        }
    }
}