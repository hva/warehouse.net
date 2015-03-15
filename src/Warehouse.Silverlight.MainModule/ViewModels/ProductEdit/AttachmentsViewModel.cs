using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Data.Interfaces;

namespace Warehouse.Silverlight.MainModule.ViewModels.ProductEdit
{
    public class AttachmentsViewModel
    {
        private string productId;
        private readonly IFilesRepository filesRepository;
        private readonly IProductsRepository productsRepository;

        public AttachmentsViewModel(IFilesRepository filesRepository, IProductsRepository productsRepository)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;

            BrowseCommand = new DelegateCommand(Browse);
        }

        public void Init(string _productId)
        {
            productId = _productId;
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
                var file = dlg.File;
                var task = await filesRepository.Create(file.OpenRead(), file.Name, "image/jpeg");
                if (task.Succeed)
                {
                    var fileId = task.Result;
                    var task2 = await productsRepository.AttachFile(productId, fileId);
                    if (task2.Succeed)
                    {
                        
                    }
                }
            }
        }
    }
}
