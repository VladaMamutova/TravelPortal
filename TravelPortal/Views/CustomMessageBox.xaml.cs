using System.Windows;
using System.Windows.Input;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private CustomMessageBox(string title, string message)
        {
            InitializeComponent();
            Title.Text = title;
            Message.Text = message;
        }

        public static void Show(string title, string message)
        {
            CustomMessageBox messageBox = new CustomMessageBox(title, message);
            messageBox.ShowDialog();
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Title_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
