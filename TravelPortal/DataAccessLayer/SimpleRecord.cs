using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelPortal.Annotations;

namespace TravelPortal.DataAccessLayer
{
    public class SimpleRecord : ITableEntry, INotifyPropertyChanged
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
        public string GetName() => Name;

        public void SetId(int id) => Id = id;

        public static readonly SimpleRecord Empty;

        static SimpleRecord() { Empty = new SimpleRecord(-1, ""); }
        
        public SimpleRecord(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public SimpleRecord(ITableEntry newRecord)
        {
            Id = newRecord.GetId();
            Name = newRecord.GetName();
        }

        public static string GenerateTitle(string propertyName = nameof(Name))
        {
            return "Наименование";
        }
        
        public virtual bool IsReadyToInsert()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }

        public virtual string GetParameterList()
        {
            return $"'{Name}'";
        }
        
        public virtual string GetIdentifiedParameterList()
        {
            return $"{Id}, {GetParameterList()}";
        }

        public virtual bool Equals(ITableEntry record)
        {
            if (record == null)
                return false;

            if (ReferenceEquals(this, record))
                return true;

            return string.Compare(Name, record.GetName(),
                       StringComparison.CurrentCulture) == 0 && Id == record.GetId();
        }

        //public virtual bool Equals(SimpleRecord record)
        //{
        //    if (record == null)
        //        return false;

        //    if (ReferenceEquals(this, record))
        //        return true;

        //    return string.Compare(Name, record.Name,
        //               StringComparison.CurrentCulture) == 0 && Id == record.Id;
        //}

        public override int GetHashCode()
        {
            return Name.GetHashCode();
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