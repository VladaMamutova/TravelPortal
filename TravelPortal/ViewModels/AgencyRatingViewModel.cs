using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MaterialDesignThemes.Wpf;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class AgencyRatingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IList<string> _headers;

        private IList<RowDataItem> _collection;
        public IList<RowDataItem> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        public int Count => Collection.Count;

        public List<string> OwnershipCollection { get; }

        public ObservableCollection<FilterListItem> Filters { get; }
        
        private FilterListItem _selectedFilter;
        public FilterListItem SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                if (_selectedFilter == null) return;

                if (SelectedFilter.Query == Queries.RankByGrossProfit("", ""))
                {
                    ControlsVisibility = Visibility.Visible;
                    UpdateCollection(Queries.RankByGrossProfit(Ownership, AgencyName));
                }
                else
                {
                    ControlsVisibility = Visibility.Collapsed;
                    UpdateCollection(_selectedFilter.Query);
                }
            }
        }

        private string _agencyName;
        public string AgencyName
        {
            get => _agencyName;
            set
            {
                _agencyName = value;
                OnPropertyChanged(nameof(AgencyName));
                UpdateCollection(Queries.RankByGrossProfit(Ownership, AgencyName));
            }
        }

        private string _ownership;
        public string Ownership
        {
            get => _ownership;
            set
            {
                _ownership = value;
                OnPropertyChanged(nameof(Ownership));
                UpdateCollection(Queries.RankByGrossProfit(Ownership, AgencyName));
            }
        }

        private Visibility _controlsVisibility;
        public Visibility ControlsVisibility
        {
            get => _controlsVisibility;
            set
            {
                _controlsVisibility = value;
                OnPropertyChanged(nameof(ControlsVisibility));
            }
        }

        private void UpdateCollection(object o)
        {
             Collection = MainTables.GetRatingCollection(o.ToString(), ref _headers);
             OnAutoGeneratingColumns(_headers);
        }

        public AgencyRatingViewModel()
        {
            _headers = new List<string>();
            Filters = new ObservableCollection<FilterListItem>
            {
                new FilterListItem("По популярности", PackIconKind.Star, Queries.RankByPopularity),
                new FilterListItem("По валовому доходу", PackIconKind.CashUsd, Queries.RankByGrossProfit("", "")),
                new FilterListItem("По количеству маршрутов", PackIconKind.MapMarkerDistance, Queries.RankByNumberOfRoutes)
            };
            OwnershipCollection =
                Dictionaries.GetNameList(DictionaryKind.Ownership);
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