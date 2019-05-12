using System;
using System.Windows;
using System.Windows.Controls;
using TravelPortal.DataAccessLayer;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для DictionariesUserControl.xaml
    /// </summary>
    public partial class DictionariesControl : UserControl
    {
        public DictionariesControl(Window owner)
        {
            InitializeComponent();
            DataContext = new DictionariesViewModel(owner);
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == nameof(Hotel.Type))
                e.Column = new DataGridTemplateColumn
                    { CellTemplate = (DataTemplate)Resources["RatingBarDataTemplate"] };

            if (e.PropertyType == typeof(DateTime) &&
                e.Column is DataGridTextColumn dateColumn)
                dateColumn.Binding.StringFormat = "dd.MM.yyyy";

            e.Column.Header =
                ((DictionaryViewModel)Dictionaries.SelectedItem).GenerateTitleFunc
                .Invoke(e.PropertyName);

            if (e.PropertyName == nameof(SimpleRecord.Name))
                e.Column.DisplayIndex = 0;
        }
    }
}
