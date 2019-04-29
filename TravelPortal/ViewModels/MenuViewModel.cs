using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using TravelPortal.Annotations;
using TravelPortal.Database;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для главного окна,
    /// которая включает представление меню и главного фрейма.
    /// </summary>
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
                // Для возвращения объекта команды используем оператор ??,
                // который называется оператором null - объединения. Если левый
                // операнд равен null "??" вернёт правый операнд, иначе - левый.
                _navigateCommand = _navigateCommand ??
                        // Создаём объект команды: 1 параметр - CommandParameter
                        // (передавали объект кнопки меню RadioButton),
                        // 2 параметр - условие выполнения команды.
                        (_navigateCommand = new RelayCommand(obj =>
                        {
                            if (obj is RadioButton page)
                            {
                                SelectedPageName = page.Content.ToString();
                                SelectedPage = Pages[SelectedPageName];
                            }
                        }, obj => Pages.Count > 0));
                return _navigateCommand;
            }
        }

        public MenuViewModel(Configuration.Roles role)
        {
            Pages = GeneratePages(role);
            if(Pages.Count == 0) throw new ArgumentException(nameof(role));
            SelectedPageName = Pages.Keys.ElementAt(0);
        }

        private Dictionary<string, Page> GeneratePages(Configuration.Roles role)
        {
            Dictionary<string, Page> pages = new Dictionary<string, Page>();
            switch (role)
            {
                // Меню рядового сотрудника туристического портала.
                case Configuration.Roles.Employee:
                {
                    pages.Add("Маршруты".ToUpper(), new RoutesPage());
                    //pages.Add("Путёвки".ToUpper(), new VouchersPage());
                    return pages;
                }
                // Меню администратора БД.
                case Configuration.Roles.Admin:
                {
                    pages.Add("Cправочные таблицы".ToUpper(), new DictionariesPage());
                    return pages;
                }
                default: return pages;
            }
        }
    }
}