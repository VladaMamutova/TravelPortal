using System.Windows.Controls;

namespace TravelPortal.ViewModels
{
    public class TabViewModel 
    {
        public string Header { get; }
        public UserControl Content { get; }

        public TabViewModel(string header, UserControl content)
        {
            Header = header;
            Content = content;
        }
    }
}