﻿using System.Collections.ObjectModel;
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
                    new DictionaryViewModel(DictionaryKind.Hotel),
                    //new DictionaryViewModel(DictionaryModels.Ticket),
                    new DictionaryViewModel(DictionaryKind.Transport),
                    new DictionaryViewModel(DictionaryKind.City),
                    new DictionaryViewModel(DictionaryKind.Ownership),
                    new DictionaryViewModel(DictionaryKind.Status)
                };
        }
    }
}