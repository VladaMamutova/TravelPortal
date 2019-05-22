
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using TravelPortal.Models;
using TravelPortal.ViewModels;

namespace TravelPortal.Views
{
    /// <summary>
    /// Логика взаимодействия для HotelRatingControl.xaml
    /// </summary>
    public partial class HotelRatingControl : UserControl
    {
        public HotelRatingControl()
        {
            InitializeComponent();
            DataContext = new HotelRatingViewModel();
        }
    }
}
