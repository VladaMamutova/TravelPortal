using System.Windows;
using System.Windows.Input;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RouteWindow.xaml
    /// </summary>
    public partial class RouteRecordDialog : Window
    {
        public RouteRecordDialog(Route route)
        {
            InitializeComponent();
            DataContext = new RouteRecordViewModel(route, this);
            ((RouteRecordViewModel) DataContext).MessageBoxDisplayRequested +=
                (sender, args) => { MessageBox.Show(args.Text, args.Title); };
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void RouteRecordDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ((RouteRecordViewModel) DataContext).Loaded();
        }
    }
}
