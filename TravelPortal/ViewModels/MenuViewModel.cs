using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TravelPortal.Annotations;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    // Модель представления для главного окна,
    // которая вклюяает представление для меню и главного фрейма.
    public class MenuViewModel : INotifyPropertyChanged
    {
        private string _selectedPageTitle; 
        private Page _selectedPage;

        public Dictionary<string, Page> Pages { get; }

        public string SelectedPageName // Выбранный пункт меню.
        {
            get => _selectedPageTitle;
            set
            {
                _selectedPageTitle = value;
                SelectedPage = Pages[_selectedPageTitle];
                OnPropertyChanged(nameof(SelectedPageName));
            }
        }

        public Page SelectedPage // Открытая страница в главном фрейме (изменяется вследствие изменения пункта меню).
        {
            get => _selectedPage;
            set
            {
                _selectedPage = value;
                OnPropertyChanged(nameof(SelectedPage));
            }
        }

        // Реализуем интерефейс INotifyPropertyChanged,
        // чтобы сообщать клиентскому окну, когда изменяются
        // свойства объекта данной модели представления.
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        // Команда перехода по пунктам меню.
        private RelayCommand _navigateCommand;
        public RelayCommand NavigateCommand
        {
            get
            {
                _navigateCommand = _navigateCommand ??
                       (_navigateCommand = new RelayCommand(obj =>
                       {
                           if (obj is string pageName)
                           {
                               SelectedPageName = pageName;
                               SelectedPage = Pages[SelectedPageName];
                           }
                       }, obj => Pages.Count > 0));
                return _navigateCommand;
            }
        }

        public MenuViewModel() // Тут будет параметризованный конструктор,
                               // в котором будут создаваться только нужны страницы.
        {
            Pages = new Dictionary<string, Page>();
            Pages.Add("Маршруты", new RoutesPage());
            Pages.Add("Путёвки", new VouchersPage());
            SelectedPageName = "Маршруты";
        }
    }
}