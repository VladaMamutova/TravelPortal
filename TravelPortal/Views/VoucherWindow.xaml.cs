using System.Windows;
using System.Windows.Input;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RouteWindow.xaml
    /// </summary>
    public partial class VoucherWindow : Window
    {
        public VoucherWindow()
        {
            InitializeComponent();
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
        {
            //DialogResult = true;
            Application.Current.Shutdown();
        }
    }
}
