using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для AddUserDialog.xaml
    /// </summary>
    public partial class UserRecordDialog : Window
    {
        public UserRecordDialog(ITableEntry record)
        {
            InitializeComponent();
            if (record is User user)
                DataContext = new UserRecordViewModel(user, this);

            ((UserRecordViewModel)DataContext).MessageBoxDisplayRequested += (sender, e) =>
            {
                CustomMessageBox.Show(e.Title, e.Text);
            };
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void PasswordConfirmationBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
           UpdateAddButtonState();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAddButtonState();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAddButtonState();
        }

        private void UpdateAddButtonState()
        {
            if (!((UserRecordViewModel)DataContext).Command.CanExecute(null))
                AddButton.IsEnabled = false;
            else
            {
                if (PasswordBox.Password.Length < 4 ||
                    PasswordConfirmationBox.Password.Length < 4 ||
                    string.CompareOrdinal(PasswordBox.Password,
                        PasswordConfirmationBox.Password) != 0)
                    AddButton.IsEnabled = false;
                else
                {
                    AddButton.IsEnabled = true;
                }
            }
        }

    }
}
