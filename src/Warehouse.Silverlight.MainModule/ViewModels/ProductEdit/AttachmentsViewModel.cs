using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Data.Products;

namespace Warehouse.Silverlight.MainModule.ViewModels.ProductEdit
{
    public class AttachmentsViewModel
    {
        private readonly IProductsRepository repository;

        public AttachmentsViewModel(IProductsRepository repository)
        {
            this.repository = repository;

            BrowseCommand = new DelegateCommand(Browse);
        }

        public void Init(string productId)
        {
        }

        public ICommand BrowseCommand { get; private set; }

        private async void Browse()
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
                var task = await repository.AddFile(dlg.File.OpenRead());
                if (task.Succeed)
                {
                    
                }
            }
        }
    }
}
