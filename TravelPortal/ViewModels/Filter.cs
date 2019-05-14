using MaterialDesignThemes.Wpf;

namespace TravelPortal.ViewModels
{
    public class Filter
    {
        public string Name { get; set; }
        public PackIconKind Icon { get; set; }
        public string Query { get; set; }

        public Filter(string name, PackIconKind icon, string query)
        {
            Name = name;
            Icon = icon;
            Query = query;
        }
    }
}