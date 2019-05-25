using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TravelPortal.Annotations;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class VoucherViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public List<string> StatusCollection { get; }
        public string SelectedStatus { get; set; }

        private Voucher _selectedItem;

        public Voucher SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private ObservableCollection<Voucher> _collection;

        public ObservableCollection<Voucher> Collection
        {
            get => _collection;
            set
            {
                _collection = value;
                OnPropertyChanged(nameof(Collection));
            }
        }

        private Window _owner;

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
                UpdateCollection();
            }
        }

        public VoucherViewModel(Window owner)
        {
            _owner = owner;
            Filters = new ObservableCollection<FilterListItem>
            {
                new FilterListItem("Все ", Queries.MainTables.GetVouchers()),
                new FilterListItem("Сегодня", Queries.Ratings.RankByGrossProfit("", "")),
                new FilterListItem("Прошедшие", Queries.Ratings.RankByNumberOfRoutes),
                new FilterListItem("Будущие", Queries.Ratings.RankByNumberOfRoutes),
            };
            SelectedFilter = Filters[0];
            StatusCollection =
                    Dictionaries.GetNameList(DictionaryKind.Status);
        }

        private void UpdateCollection()
        {
            try
            {
                Collection = MainTables.GetVouchers(_selectedFilter.Query);
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest("Ошибка получения списка путёвок", ex.Message);
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
    }
}