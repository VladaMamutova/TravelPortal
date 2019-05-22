using MaterialDesignThemes.Wpf;

namespace TravelPortal.ViewModels
{
    public class RatingFilter
    {
        public string Name { get; set; }
        public PackIconKind Icon { get; set; }
        public string Query { get; set; }

        public RatingFilter(string name, PackIconKind icon, string query)
        {
            Name = name;
            Icon = icon;
            Query = query;
        }
    }
}