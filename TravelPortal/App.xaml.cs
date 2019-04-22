using System.Windows;
using TravelPortal.Views;

namespace TravelPortal
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = new MainWindow();


            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
                MainWindow.Show();
            else MainWindow.Close();
        }
    }
}
