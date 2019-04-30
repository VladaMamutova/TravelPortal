namespace TravelPortal.Models
{
    public class Hotel : SimpleRecord
    {
        public int Type { get; set; }

        public new static Hotel Empty;

        static Hotel() { Empty = new Hotel(-1, "", 0); }

        public Hotel(Hotel newRecord) : base(newRecord.GetId(), newRecord.Name)
        {
            Type = newRecord.Type;
        }

        public Hotel(int id, string name, int type) : base(id, name)
        {
            Type = type;
        }

        public new static string GenerateTitle(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(Name): return "Название";
                case nameof(Type): return "Категория";
                default: return propertyName;
            }
        }

        public new bool Equals(SimpleRecord other)
        {
            if (other == null)
                return false;

            if (GetType() != other.GetType())
                return false;

            return base.Equals(other) && Type == ((Hotel) other).Type;
        }

        public override int GetHashCode()
        {
            return (Id + Name).GetHashCode();
        }

        public override string ToString()
        {
            return $"id={Id}, Name=\"{Name}\"";
        }
    }
}