using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Printing;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.Attachments
{
    public class AttachmentDetailViewModel : Notification
    {
        public AttachmentDetailViewModel(FileDescription file)
        {
            Title = file.Name;
            Uri = string.Concat(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString(), "api/files/", file.Id);
            PrintCommand = new DelegateCommand<ChildWindow>(Print);
        }

        public string Uri { get; private set; }
        public ICommand PrintCommand { get; private set; }

        private void Print(ChildWindow window)
        {
            var doc = new PrintDocument();

            var uri = new Uri(Uri, UriKind.Absolute);

            doc.PrintPage += (s, ea) =>
            {
                ea.PageVisual = new Image { Source = new BitmapImage(uri) };
                ea.HasMorePages = false;
            };

            doc.EndPrint += (sender, args) => window.Close();

            var settings = new PrinterFallbackSettings { ForceVector = false };

            doc.Print(Title, settings);
        }
    }
}
