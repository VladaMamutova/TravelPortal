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
    public partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            InitializeComponent();

            RatingViewModel model = new RatingViewModel();
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

            InitializeControls();
            DataContext = model;
        }

        private void InitializeControls()
        {
            var textBox = new TextBox {MinWidth = 150, MaxWidth = 300};
            textBox.SetBinding(TextBox.TextProperty,
                new Binding(nameof(RatingViewModel.AgencyName))
                {
                    UpdateSourceTrigger =
                        UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                });
            HintAssist.SetHint(textBox,
                Agency.GenerateTitle(nameof(Agency.Name)));
            ControlsPanel.Children.Add(textBox);

            var comboBox = new ComboBox
                {Margin = new Thickness(10, 0, 0, 0)};

            comboBox.SetBinding(Selector.SelectedItemProperty,
                new Binding(nameof(RatingViewModel.Ownership))
                {
                    UpdateSourceTrigger =
                        UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                });
            HintAssist.SetHint(comboBox, "Тип собственности");
            comboBox.ItemsSource =
                Dictionaries.GetNameList(DictionaryKind.Ownership);
            ControlsPanel.Children.Add(comboBox);
        }
    }
}
