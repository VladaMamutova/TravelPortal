using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MaterialDesignThemes.Wpf;
using TravelPortal.Annotations;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class RatingViewModel : ViewModelBase, INotifyPropertyChanged
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

        public ObservableCollection<Filter> Filters { get; }
        
        private Filter _selectedFilter;
        public Filter SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                if (_selectedFilter == null) return;

                ControlsVisibility =
                    SelectedFilter.Query == Queries.RankByGrossProfit("", "")
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                UpdateCollection(_selectedFilter.Query);
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

        public RatingViewModel()
        {
            _headers = new List<string>();
            Filters = new ObservableCollection<Filter>
            {
                new Filter("По популярности", PackIconKind.Star, Queries.RankByPopularity),
                new Filter("По валовому доходу", PackIconKind.CashUsd, Queries.RankByGrossProfit("", "")),
                new Filter("По количеству маршрутов", PackIconKind.MapMarkerDistance, Queries.RankByNumberOfRoutes)
            };
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