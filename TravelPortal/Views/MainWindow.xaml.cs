using System.Windows;
using System.Windows.Controls;
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

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new MainViewModel(this);
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void ButtonExpand_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                ((Button)sender).ToolTip = "Развернуть";
                WindowHeader.Margin = new Thickness(0);
            }
            else
            {
                WindowState = WindowState.Maximized;
                ((Button)sender).ToolTip = "Свернуть";
                WindowHeader.Margin = new Thickness(10);
            }
        }
    }
}
