using System.Windows;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionaryRecordDialog.xaml
    /// </summary>
    public partial class DictionaryRecordDialog : Window
    {
        public DictionaryRecordDialog()
        {
            InitializeComponent();
        }

        private void Move(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
