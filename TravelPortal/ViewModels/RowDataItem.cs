using System.Collections.Generic;

namespace TravelPortal.ViewModels
{
    public class RowDataItem
    {
        public IList<string> Fields { get; }

        public RowDataItem(List<string> fields)
        {
            Fields = fields;
        }
    }
}
