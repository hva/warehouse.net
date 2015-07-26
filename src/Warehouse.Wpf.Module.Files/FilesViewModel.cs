using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class FilesViewModel : BindableBase, INavigationAware
    {
        private FileDescription[] items;
        private IDictionary<string, string> names;
        private bool isBusy;
        private IList selectedItems;
        private readonly DelegateCommand deleteCommand;
        private readonly InteractionRequest<Confirmation> deleteRequest;
        private readonly InteractionRequest<IConfirmation> editRequest;

        private readonly IFilesRepository filesRepository;
        private readonly IProductsRepository productsRepository;
        private readonly Func<CreateFileViewModel> createFactory;
        private readonly Func<EditFileViewModel> editFactory;

        public FilesViewModel(IFilesRepository filesRepository, IAuthStore authStore, IProductsRepository productsRepository, 
            Func<CreateFileViewModel> createFactory, Func<EditFileViewModel> editFactory)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;
            this.createFactory = createFactory;
            this.editFactory = editFactory;

            var token = authStore.LoadToken();
            if (token != null)
            {
                IsEditor = token.IsEditor();
                IsAdmin = token.IsAdmin();
            }

            editRequest = new InteractionRequest<IConfirmation>();
            BrowseCommand = new DelegateCommand(Browse);
            EditCommand = new DelegateCommand<FileDescription>(Edit);

            deleteCommand = new DelegateCommand(PromtDelete, HasSelectedItems);
            deleteRequest = new InteractionRequest<Confirmation>();
        }

        public bool IsEditor { get; private set; }
        public bool IsAdmin { get; private set; }
        
        public ICommand DeleteCommand { get { return deleteCommand; } }
        public ICommand BrowseCommand { get; private set; }
        public IInteractionRequest DeleteRequest { get { return deleteRequest; } }
        public ICommand EditCommand { get; private set; }
        public IInteractionRequest EditRequest { get { return editRequest; } }

        public FileDescription[] Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public IList SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                deleteCommand.RaiseCanExecuteChanged();
            }
        }

        public async void OnNavigatedTo(object param)
        {
            await RefreshAsync();
        }

        public void OnNavigatedFrom()
        {
        }

        private bool HasSelectedItems()
        {
            return selectedItems != null && selectedItems.OfType<FileDescription>().Any();
        }

        private async Task RefreshAsync()
        {
            IsBusy = true;

            var task = await filesRepository.GetAll();

            if (task.Succeed)
            {
                var task2 = await productsRepository.GetNamesAsync();
                if (task2.Succeed)
                {
                    names = task2.Result.ToDictionary(x => x.Id, x => x.Name);
                }

                PopulateNames(task.Result);

                Items = task.Result;
            }

            IsBusy = false;
        }

        private async void OnFileUpdated(IConfirmation context)
        {
            if (context.Confirmed)
            {
                await RefreshAsync();
            }
        }

        #region Delete

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
                IsBusy = true;

                var ids = selectedItems.OfType<FileDescription>().Select(x => x.Id).ToArray();
                var task = await filesRepository.DeleteAsync(ids);
                if (task.Succeed)
                {
                    await RefreshAsync();
                }

                IsBusy = false;
            }
        }

        #endregion

        private void PopulateNames(IEnumerable<FileDescription> files)
        {
            if (names != null && files != null)
            {
                foreach (var x in files)
                {
                    if (x.Metadata != null)
                    {
                        var sb = new StringBuilder();
                        foreach (var id in x.Metadata.ProductIds)
                        {
                            string name;
                            if (names.TryGetValue(id, out name))
                            {
                                sb.AppendLine(name);
                            }
                        }
                        x.Metadata.ProductNames = sb.ToString().TrimEnd();
                    }
                }
            }
        }

        private void Browse()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "JPEG Images (*.jpg, *.jpeg)|*.jpg;*.jpeg"
            };

            if (dialog.ShowDialog() == true)
            {
                var context = createFactory();
                context.Init(dialog);
                editRequest.Raise(context, OnFileUpdated);
            }
        }

        private void Edit(FileDescription file)
        {
            var context = editFactory();
            context.Init(file, names);
            editRequest.Raise(context, OnFileUpdated);
        }
    }
}
