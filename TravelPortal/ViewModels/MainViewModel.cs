using System.Collections.ObjectModel;
using System.Windows;
using TravelPortal.Database;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для главного окна,
    /// которая включает представление меню и главного фрейма.
    /// </summary>
    public class MainViewModel
    {
        public ObservableCollection<TabViewModel> Tabs { get; }
        
        public MainViewModel(Window owner)
        {
            switch (Configuration.Role)
            {
                // Меню рядового сотрудника туристического портала.
                case Configuration.Roles.Employee:
                {
                    Tabs = new ObservableCollection<TabViewModel>()
                    {
                        new TabViewModel("Маршруты".ToUpper(), new RoutesControl(owner)),
                        new TabViewModel("Путёвки".ToUpper(), new VouchersControl(owner))
                    };
                    break;
                    }
                // Меню администратора БД.
                case Configuration.Roles.Admin:
                {
                    Tabs = new ObservableCollection<TabViewModel>()
                    {
                        new TabViewModel("Cправочные таблицы".ToUpper(), new DictionariesControl(owner))
                    };
                    break;
                }
                default: Tabs = new ObservableCollection<TabViewModel>(); break;
            }
        }
    }
}