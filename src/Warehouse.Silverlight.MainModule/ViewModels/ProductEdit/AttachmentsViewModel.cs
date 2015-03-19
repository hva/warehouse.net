using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels.ProductEdit
{
    public class AttachmentsViewModel : NotificationObject
    {
        private string productId;
        private object[] selectedItems;
        private Uri image;
        private readonly IFilesRepository filesRepository;
        private readonly IProductsRepository productsRepository;

        public AttachmentsViewModel(IFilesRepository filesRepository, IProductsRepository productsRepository)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;

            BrowseCommand = new DelegateCommand(Browse);
            Files = new ObservableCollection<FileInfo>();
        }

        public async Task Init(string _productId)
        {
            productId = _productId;

            await LoadFiles();
        }

        public ICommand BrowseCommand { get; private set; }

        public ObservableCollection<FileInfo> Files { get; private set; }

        public object[] SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                Preview();
            }
        }

        public Uri Image
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
                Files.AddRange(task.Result);
            }
        }

        private void Preview()
        {
            if (selectedItems != null && selectedItems.Length > 0)
            {
                var fileInfo = selectedItems[0] as FileInfo;
                if (fileInfo != null)
                {
                    var uriString = string.Concat(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString(), "api/files/", fileInfo.Id);
                    Image = new Uri(uriString, UriKind.Absolute);
                }
            }
        }
    }
}
