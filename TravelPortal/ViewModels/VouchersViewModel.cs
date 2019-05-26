using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;

namespace TravelPortal.ViewModels
{
    public class VouchersViewModel : ViewModelBase
    {
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

        private List<string> _statusCollection;
        public List<string> StatusCollection
        {
            get => _statusCollection;
            set
            {
                _statusCollection = value;
                OnPropertyChanged(nameof(StatusCollection));
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
        
        public ObservableCollection<FilterListItem> Filters { get; }

        private FilterListItem _selectedFilter;
        public FilterListItem SelectedFilter
        {
            get => _selectedFilter;
            set
            {
                if (value == null) return;
                _selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));
                UpdateCollection();
            }
        }

        private RelayCommand _cancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                _cancelCommand = _cancelCommand ?? new RelayCommand(o =>
                {
                    try
                    {
                        MainTables.Execute(Queries.MainTables.CancelVoucher(SelectedItem.GetId()));
                        UpdateCollection();
                    }
                    catch (Exception ex)
                    {
                        OnMessageBoxDisplayRequest("Ошибка отмены путёвки", ex.Message);
                    }
                }, o => SelectedItem != null);
                return _cancelCommand;
            }
        }

        public VouchersViewModel()
        {
            Filters = new ObservableCollection<FilterListItem>
            {
                new FilterListItem("Все ", Queries.MainTables.GetVouchers),
                new FilterListItem("Прошедшие", Queries.MainTables.GetPastVouchers),
                new FilterListItem("Сегодня", Queries.MainTables.GetTodayVouchers),
                new FilterListItem("Завтра", Queries.MainTables.GetTomorrowVouchers),
                new FilterListItem("Будущие", Queries.MainTables.GetFutureVouchers),
            };
        }

        public void LoadFromDb()
        {
            SelectedFilter = Filters[2];
            StatusCollection =
                Dictionaries.GetNameList(DictionaryKind.Status);
        }

        private void UpdateCollection()
        {
            try
            {
                Collection = MainTables.GetVouchers(_selectedFilter.Query);
                OnCollectionChanged(new ObservableCollection<object>(_collection));
            }
            catch (Exception ex)
            {
                OnMessageBoxDisplayRequest("Ошибка получения списка путёвок", ex.Message);
            }
        }
    }
}