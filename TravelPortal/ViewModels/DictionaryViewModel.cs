using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Npgsql;
using NpgsqlTypes;
using TravelPortal.Annotations;
using TravelPortal.Database;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    // Для полной поддержки передачи значений данных от объектов источника
    // для целевых объектов, каждый объект в коллекции, который поддерживает
    // свойства связывания должен также реализовывать INotifyPropertyChanged
    // интерфейс. 
    // Необходимо будет реализовать этот интерфейс, если нужно будет
    // гарантировать изменение и перерисовку таблицы после изменения
    // свойства одного элемента коллекции (т.е. изменить таблицу с Agencies
    // на форме после изменения Agencies[0].Name, например. Если Agencies[0]
    // заменить на новый объект (Agencies[0] = new Agency()), интерфейс
    // реализовывать не надо).
    public class DictionaryViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Agency> _agencies;
    
        public ObservableCollection<Agency> Agencies {
            get => _agencies;
            set
            {
                _agencies = value;
                OnPropertyChanged(nameof(Agencies));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        public DictionaryViewModel()
        {
            Agencies = new ObservableCollection<Agency>(); using (var connection =
                new NpgsqlConnection(Configuration.GetConnetionString()))
            {
                connection.Open();

                Agencies = GetAgencies(connection);
            }
        }

        private static ObservableCollection<Agency> GetAgencies(NpgsqlConnection npgsqlConnection)
        {
            using (var command = new NpgsqlCommand(
                Queries.Dictionaries.SelectAllFromAgencies, npgsqlConnection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) return null;
                    ObservableCollection<Agency> collection = new ObservableCollection<Agency>();
                    while (reader.Read())
                    {
                        int agencyId = reader.GetInt32(0);
                        string registration = reader.GetString(1).TrimEnd();
                        string name = reader.GetString(2).TrimEnd();
                        string city = reader.GetString(3).TrimEnd();
                        string address = reader.GetString(4).TrimEnd();
                        string ownership = reader.GetString(5).TrimEnd();
                        string phone = reader.GetString(6).TrimEnd();
                        NpgsqlDate date = reader.GetDate(7);
                        collection.Add(new Agency(agencyId, registration, name,
                            city, address, ownership, phone, date));
                    }

                    return collection;
                }
            }
        }
    }
}