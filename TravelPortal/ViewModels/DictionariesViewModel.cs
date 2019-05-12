using System.Collections.ObjectModel;
using System.Windows;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.ViewModels
{
    public class DictionariesViewModel
    {
        public ObservableCollection<DictionaryViewModel> DictionariesTabs { get; }

        public DictionariesViewModel(Window owner)
        {
            //if (Configuration.Role == Configuration.Roles.Admin){}
          
            DictionariesTabs =
                new ObservableCollection<DictionaryViewModel>
                {
                    new DictionaryViewModel(DictionaryKind.Agency, owner),
                    new DictionaryViewModel(DictionaryKind.Hotel, owner),
                    new DictionaryViewModel(DictionaryKind.Ticket, owner),
                    new DictionaryViewModel(DictionaryKind.Transport, owner),
                    new DictionaryViewModel(DictionaryKind.City, owner),
                    new DictionaryViewModel(DictionaryKind.Ownership, owner),
                    new DictionaryViewModel(DictionaryKind.Status, owner)
                };
        }
    }
}