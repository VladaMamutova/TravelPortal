using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TravelPortal.Database;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void InitializeMenu()
        {
            // В зависимости от роли пользователя
            // генерируем соответствующие пункты меню.

            MenuViewModel menu = new MenuViewModel(Configuration.Role);
            RadioButton[] menuItems = new RadioButton[menu.Pages.Count];
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i] = new RadioButton
                {
                    Margin = new Thickness(4),
                    Content = menu.Pages.Keys.ElementAt(i),
                    Command = menu.NavigateCommand,
                    CommandParameter =  RelativeSource.Self
                };
                menuItems[i].CommandParameter = menuItems[i];
            }
            menuItems.ElementAt(0).IsChecked = true;

            foreach (var menuItem in menuItems)
                TabMenu.Children.Add(menuItem);

            DataContext = menu;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeMenu();
        }
    }
}
