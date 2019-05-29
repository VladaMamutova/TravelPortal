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
        public string CurrentUserName { get; }
        public string CurrentAgency { get; }
        public Visibility AgencyVisibility { get; }

        public ObservableCollection<TabViewModel> Tabs { get; }
        
        public MainViewModel(Window owner)
        {
            Configuration configuration = Configuration.GetConfiguration();
            CurrentUserName = configuration.CurrentUser?.Name ?? "";
            if (!string.IsNullOrEmpty(configuration.CurrentUser?.Agency))
            {
                CurrentAgency = configuration.CurrentUser.Agency;
                AgencyVisibility = Visibility.Visible;
            }
            else AgencyVisibility = Visibility.Hidden;

            // Генерация меню в зависимости от роли текущего пользователя.
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
                            new VouchersControl()),
                        new TabViewModel("Клиенты".ToUpper(),
                            new CustomersControl(owner))
                    };
                    break;
                }
                // Меню супервайзера.
                case Roles.Supervisor:
                {
                    Tabs = new ObservableCollection<TabViewModel>
                    {
                        new TabViewModel("Рейтинг агентств".ToUpper(),
                            new AgencyRatingControl()),
                        new TabViewModel("Рейтинг отелей".ToUpper(),
                            new HotelRatingControl()),
                    };
                    break;
                }

                // Меню администратора комплекса.
                case Roles.Admin:
                {
                    Tabs = new ObservableCollection<TabViewModel>
                    {
                        new TabViewModel("Сотрудники портала".ToUpper(),
                            new UsersControl(owner)),
                        new TabViewModel("Cправочные таблицы".ToUpper(),
                            new DictionariesControl(owner))
                    };
                    break;
                }
                default: Tabs = new ObservableCollection<TabViewModel>(); break;
            }
        }
    }
}