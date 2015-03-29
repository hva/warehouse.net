using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
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
        private readonly InteractionRequest<AttachmentDetailViewModel> openDetailRequest;
        private readonly InteractionRequest<Confirmation> deleteRequest;
        private readonly DelegateCommand deleteCommand;
        private IList selectedItems;
        private bool isBusy;

        public AttachmentsViewModel(IFilesRepository filesRepository, IProductsRepository productsRepository)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;

            BrowseCommand = new DelegateCommand(Browse);
            OpenFileCommand = new DelegateCommand<FileDescription>(OpenFile);
            Files = new ObservableCollection<FileDescription>();
            openDetailRequest = new InteractionRequest<AttachmentDetailViewModel>();
            deleteCommand = new DelegateCommand(PromtDelete, CanDelete);
            deleteRequest = new InteractionRequest<Confirmation>();
        }

        public async Task Init(string _productId)
        {
            productId = _productId;

            await LoadFiles();
        }

        public ICommand BrowseCommand { get; private set; }
        public ICommand OpenFileCommand { get; private set; }
        public ICommand DeleteCommand { get { return deleteCommand; } }
        public IInteractionRequest OpenDetailRequest { get { return openDetailRequest; } }
        public IInteractionRequest DeleteRequest { get { return deleteRequest; } }
        public ObservableCollection<FileDescription> Files { get; private set; }

        public IList SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                deleteCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
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

                var progress = new ProgressWindow { Content = "Закачиваем файл..." };
                progress.Show();

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

                progress.Close();
            }
        }

        private async Task LoadFiles()
        {
            Files.Clear();

            IsBusy = true;
            var task = await productsRepository.GetFiles(productId);
            IsBusy = false;

            if (task.Succeed)
            {
                var files = task.Result.OrderByDescending(x => x.UploadDate);
                Files.AddRange(files);
            }
        }

        private void OpenFile(FileDescription file)
        {
            openDetailRequest.Raise(new AttachmentDetailViewModel(file));
        }

        private bool CanDelete()
        {
            return selectedItems != null && selectedItems.Count > 0;
        }

        private void PromtDelete()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Следующие файлы будут удалены:");
            foreach (var x in selectedItems.OfType<FileDescription>())
            {
                sb.AppendFormat("- {0}", x.Name);
                sb.AppendLine();
            }

            var conf = new Confirmation
            {
                Title = "Внимание!",
                Content = sb.ToString(),
            };

            deleteRequest.Raise(conf, Delete);
        }

        private async void Delete(Confirmation conf)
        {
            if (conf.Confirmed)
            {
                var ids = selectedItems
                    .OfType<FileDescription>()
                    .OrderByDescending(x => x.UploadDate)
                    .Select(x => x.Id)
                    .ToArray();

                IsBusy = true;
                var task = await productsRepository.DetachFiles(productId, ids);
                IsBusy = false;

                if (task.Succeed)
                {
                    await LoadFiles();
                }
            }
        }
    }
}
