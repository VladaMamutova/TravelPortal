using System.Collections.ObjectModel;
using System.Windows;
using TravelPortal.Models;
using TravelPortal.Views;

namespace TravelPortal.ViewModels
{
    /// <summary>
    /// Модель представления для главного окна,
    /// которая включает представление меню и главного фрейма.
    /// </summary>
    public class MainViewModel
    {
        public string CurrentUserName => Configuration.GetConfiguration().CurrentUser?.Name ?? "";
        public string CurrentAgency => Configuration.GetConfiguration().CurrentUser?.Agency ?? "";

        public ObservableCollection<TabViewModel> Tabs { get; }
        
        public MainViewModel(Window owner)
        {
            switch (Configuration.GetConfiguration().CurrentUser.Role)
            {
                // Меню рядового сотрудника туристического портала.
                case Roles.Employee:
                {
                    Tabs = new ObservableCollection<TabViewModel>
                    {
                        new TabViewModel("Маршруты".ToUpper(),
                            new RoutesControl(owner)),
                        new TabViewModel("Путёвки".ToUpper(),
                            new VouchersControl(owner)),
                        new TabViewModel("Клиенты".ToUpper(),
                            new CustomersControl(owner))
                    };
                    break;
                }
                // Меню администратора БД.
                case Roles.Admin:
                {
                    Tabs = new ObservableCollection<TabViewModel>()
                    {
                        new TabViewModel("Cотрудники портала".ToUpper(), new EmployeesControl(owner)),
                        new TabViewModel("Cправочные таблицы".ToUpper(), new DictionariesControl(owner))
                    };
                    break;
                }
                default: Tabs = new ObservableCollection<TabViewModel>(); break;
            }
        }
    }
}