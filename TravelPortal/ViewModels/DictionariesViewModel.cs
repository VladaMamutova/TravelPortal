using System.Collections.ObjectModel;
using System.Windows;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.ViewModels
{
    public class DictionariesViewModel
    {
        public ObservableCollection<DictionaryViewModel> DictionariesTabs { get; }

        public DictionariesViewModel()
        {
            DictionariesTabs =
                new ObservableCollection<DictionaryViewModel>
                {
                    new DictionaryViewModel(DictionaryKind.Agency),
                    new DictionaryViewModel(DictionaryKind.Hotel),
                    new DictionaryViewModel(DictionaryKind.Ticket),
                    new DictionaryViewModel(DictionaryKind.Transport),
                    new DictionaryViewModel(DictionaryKind.City),
                    new DictionaryViewModel(DictionaryKind.Ownership),
                    new DictionaryViewModel(DictionaryKind.Status)
                };
        }
    }
}