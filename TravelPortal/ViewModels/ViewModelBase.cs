using System;
using System.Collections.Generic;
using TravelPortal.DataAccessLayer;

namespace TravelPortal.ViewModels
{
    public class ViewModelBase
    {
        // Классы, представляюшие аргументы для событий.
        public class DialogDisplayEventArgs : EventArgs
        {
            public ITableEntry Record { get; set; }
        }

        public class MessageBoxDisplayEventArgs : EventArgs
        {
            public string Title { get; set; }
            public string Text { get; set; }
        }

        public class DataGridAutoGeneratingColumnEventArgs : EventArgs
        {
            public IList<string> Headers { get; set; }
        }

        // Делегаты для обработки событий.
        public event EventHandler<DialogDisplayEventArgs>
            DialogDisplayRequested;
        public event EventHandler<MessageBoxDisplayEventArgs>
            MessageBoxDisplayRequested;
        public event EventHandler<DataGridAutoGeneratingColumnEventArgs>
            AutoGeneratingColumns;


        // Методы, выполняющиеся в событиях.
        protected void OnDialogDisplayRequest(ITableEntry record)
        {
            DialogDisplayRequested?.Invoke(
                this,
                new DialogDisplayEventArgs { Record = record });
        }

        protected void OnMessageBoxDisplayRequest(string title, string text)
        {
            MessageBoxDisplayRequested?.Invoke(
                this,
                new MessageBoxDisplayEventArgs
                {
                    Title = title,
                    Text = text
                });
        }

        protected void OnAutoGeneratingColumns(IList<string> headers)
        {
            AutoGeneratingColumns?.Invoke(
                this,
                new DataGridAutoGeneratingColumnEventArgs { Headers = headers });
        }
    }
}