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
                    new DictionaryViewModel(DictionaryModels.Transport),
                    new DictionaryViewModel(DictionaryModels.City)
                };
        }
    }
}