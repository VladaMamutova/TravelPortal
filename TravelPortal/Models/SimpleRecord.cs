using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelPortal.Annotations;

namespace TravelPortal.Models
{
    public class SimpleRecord : INotifyPropertyChanged
    {
        protected int Id;
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int GetId() => Id;
        public void SetId(int id) => Id = id;

        public static readonly SimpleRecord Empty;

        static SimpleRecord() { Empty = new SimpleRecord(-1, ""); }
        
        public SimpleRecord(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public SimpleRecord(SimpleRecord newRecord)
        {
            Id = newRecord.Id;
            Name = newRecord.Name;
        }

        public static string GenerateTitle(string propertyName = nameof(Name))
        {
            return "Наименование";
        }

        public bool Equals(SimpleRecord other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Name == other.Name && Id == other.Id;
        }

        public override int GetHashCode()
        {
            return (Id + Name).GetHashCode();
        }

        public override string ToString()
        {
            return $"id={Id}, Name=\"{Name}\"";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}