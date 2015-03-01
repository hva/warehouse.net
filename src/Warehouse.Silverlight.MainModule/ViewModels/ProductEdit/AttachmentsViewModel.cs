using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Warehouse.Silverlight.MainModule.ViewModels.ProductEdit
{
    public class AttachmentsViewModel
    {
        public AttachmentsViewModel()
        {
            BrowseCommand = new DelegateCommand(Browse);
        }

        public void Init(string productId)
        {
        }

        public ICommand BrowseCommand { get; private set; }

        private void Browse()
        {
            var dlg = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "JPEG Images (*.jpg, *.jpeg)|*.jpg;*.jpeg"
            };

            if (dlg.ShowDialog() == true)
            {
                //UploadFile(dlg.File.Name, dlg.File.OpenRead());
                //StatusText.Text = dlg.File.Name;
            }
            else
            {
                //StatusText.Text = "No file selected...";
            }
        }
    }
}
