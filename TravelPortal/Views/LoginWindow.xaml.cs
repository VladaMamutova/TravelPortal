using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelPortal.Models;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            //Login.Text = "vlada";
            //Password.Password = "admin";
            //SignIn_OnClick(SignIn, new RoutedEventArgs());
        }

        private void Power_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }

        private void Window_MouseLeftButtonDown(object sender,
            MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void SignIn_OnClick(object sender, RoutedEventArgs e)
        {

            string login = Login.Text;
            string password = Password.Password;
            Thread connectThread = new Thread(() =>
                {
                    try
                    {
                        Configuration.GetConfiguration().SetUser(login, password);
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show("Вы успешно вошли в систему!");
                            DialogResult = true;
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.InnerException?.Message,
                            "Ошибка входа");
                    }
                });
            connectThread.Start();
        }

        private void Login_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            SignIn.IsEnabled = Login?.Text.TrimEnd() != "" && Password?.Password != "";
        }

        private void Password_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            SignIn.IsEnabled = Login?.Text.TrimEnd() != "" && Password?.Password != "";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Если нажата клавиша "Enter" на любом поле ввода и кнопка "Подключиться" доступна,
            // то программно нажимаем на эту кнопку.
            if (e.Key == Key.Enter && SignIn.IsEnabled)
                SignIn_OnClick(SignIn, new RoutedEventArgs());
        }
    }
}
