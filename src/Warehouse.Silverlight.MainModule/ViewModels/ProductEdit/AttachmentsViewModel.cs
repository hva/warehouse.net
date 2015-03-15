using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels.ProductEdit
{
    public class AttachmentsViewModel : NotificationObject
    {
        private string productId;
        private FileInfo[] files;
        private IList selectedItems;
        private Image image;
        private readonly IFilesRepository filesRepository;
        private readonly IProductsRepository productsRepository;

        public AttachmentsViewModel(IFilesRepository filesRepository, IProductsRepository productsRepository)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;

            BrowseCommand = new DelegateCommand(Browse);
        }

        public async Task Init(string _productId)
        {
            productId = _productId;

            await LoadFiles();
        }

        public ICommand BrowseCommand { get; private set; }

        public FileInfo[] Files
        {
            get { return files; }
            set { files = value; RaisePropertyChanged(() => Files); }
        }

        public IList SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                Preview();
            }
        }

        public Image Image
        {
            get { return image; }
            set { image = value; RaisePropertyChanged(() => Image); }
        }

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
                        await LoadFiles();
                    }
                }
            }
        }

        private async Task LoadFiles()
        {
            var task = await productsRepository.GetFiles(productId);
            if (task.Succeed)
            {
                Files = task.Result;
            }
        }

        private void Preview()
        {
            if (selectedItems != null && selectedItems.Count > 0)
            {
                var fileInfo = selectedItems[0] as FileInfo;
                if (fileInfo != null)
                {
                    var uriString = string.Concat(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString(), "api/files/", fileInfo.Id);
                    var uri = new Uri(uriString, UriKind.Absolute);
                    Image = new Image { Source = new BitmapImage(uri)};
                }
            }
        }
    }
}
