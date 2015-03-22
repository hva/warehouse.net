using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.Attachments
{
    public class AttachmentsViewModel : NotificationObject
    {
        private string productId;
        private readonly IFilesRepository filesRepository;
        private readonly IProductsRepository productsRepository;

        public AttachmentsViewModel(IFilesRepository filesRepository, IProductsRepository productsRepository)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;

            BrowseCommand = new DelegateCommand(Browse);
            OpenFileCommand = new DelegateCommand<FileDescription>(OpenFile);
            Files = new ObservableCollection<FileDescription>();
        }

        public async Task Init(string _productId)
        {
            productId = _productId;

            await LoadFiles();
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }

        public ObservableCollection<FileDescription> Files { get; private set; }

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
            Files.Clear();
            var task = await productsRepository.GetFiles(productId);
            if (task.Succeed)
            {
                var files = task.Result.OrderByDescending(x => x.UploadDate);
                Files.AddRange(files);
            }
        }

        private void OpenFile(FileDescription file)
        {
            
        }
    }
}
