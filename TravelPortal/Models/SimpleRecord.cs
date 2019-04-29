using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelPortal.Annotations;

namespace TravelPortal.Models
{
    public class SimpleRecord : INotifyPropertyChanged
    {
        private readonly int _id;
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

        public int GetId() => _id;
        public SimpleRecord() { }

        public SimpleRecord(int id, string name)
        {
            _id = id;
            Name = name;
        }

        public SimpleRecord(SimpleRecord newRecord)
        {
            _id = newRecord._id;
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

            return Name == other.Name && _id == other._id;
        }

        public override int GetHashCode()
        {
            return (_id + Name).GetHashCode();
        }

        public override string ToString()
        {
            return $"id={_id}, Name={Name}";
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