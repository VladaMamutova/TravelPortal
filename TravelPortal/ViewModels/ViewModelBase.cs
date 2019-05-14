using System;
using System.Collections.Generic;

namespace TravelPortal.ViewModels
{
    public class ViewModelBase
    {
        public class DataGridAutoGeneratingColumnEventArgs : EventArgs
        {
            public IList<string> Headers { get; set; }
        }

        public event EventHandler<DataGridAutoGeneratingColumnEventArgs>
            AutoGeneratingColumns;

        protected void OnAutoGeneratingColumns(IList<string> headers)
        {
            AutoGeneratingColumns?.Invoke(
                this,
                new DataGridAutoGeneratingColumnEventArgs {Headers = headers});
        }
    }
}