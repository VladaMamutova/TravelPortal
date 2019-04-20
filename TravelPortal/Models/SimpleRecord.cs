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
    }
}