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
    public class VoucherViewModel : INotifyPropertyChanged
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

        public int Count => Collection?.Count ?? 0;
        private Window _owner;

        public VoucherViewModel(Window owner)
        {
            _owner = owner;
            StatusCollection =
                    Dictionaries.GetNameList(DictionaryKind.Status);
            Collection =
                MainTables.GetVouchers(Queries.MainTables.GetVouchers());
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