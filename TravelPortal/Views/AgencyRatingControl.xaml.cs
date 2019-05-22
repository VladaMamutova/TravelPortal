using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using TravelPortal.DataAccessLayer;
using TravelPortal.Models;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для RatingControl.xaml
    /// </summary>
    public partial class AgencyRatingControl : UserControl
    {
        public AgencyRatingControl()
        {
            InitializeComponent();

            AgencyRatingViewModel model = new AgencyRatingViewModel();
            model.AutoGeneratingColumns += (sender, e) =>
            {
                Grid.Columns.Clear();
                int columnIndex = 0;
                foreach (var name in e.Headers)
                {
                    Grid.Columns.Add(
                        new DataGridTextColumn
                        {
                            Header = name,
                            Binding = new Binding(nameof(RowDataItem.Fields) +
                                                  $"[{columnIndex++}]")
                        });
                }
            };
            model.SelectedFilter = model.Filters.Count > 0 ? model.Filters[0] : null;
            DataContext = model;
        }
    }
}
