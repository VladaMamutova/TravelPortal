using System.Collections.ObjectModel;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class DictionariesViewModel
    {
        public ObservableCollection<DictionaryViewModel> DictionariesTabs { get; }

        public DictionariesViewModel()
        {
            //if (Configuration.Role == Configuration.Roles.Admin){}
          
            DictionariesTabs =
                new ObservableCollection<DictionaryViewModel>
                {
                    //new DictionaryViewModel(DictionaryModels.Agency),
                    //new DictionaryViewModel(DictionaryModels.Hotel),
                    //new DictionaryViewModel(DictionaryModels.Ticket),
                    new DictionaryViewModel(DictionaryModels.Transport),
                    new DictionaryViewModel(DictionaryModels.City),
                    new DictionaryViewModel(DictionaryModels.Ownership),
                    new DictionaryViewModel(DictionaryModels.Status)
                };
        }
    }
}