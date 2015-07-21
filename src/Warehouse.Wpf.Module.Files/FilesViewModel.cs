using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
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
        private Dictionary<string, string> names;
        private bool isBusy;
        private IList selectedItems;
        private readonly DelegateCommand deleteCommand;
        private readonly InteractionRequest<Confirmation> deleteRequest;

        private readonly IFilesRepository filesRepository;
        private readonly IProductsRepository productsRepository;

        public FilesViewModel(IFilesRepository filesRepository, IAuthStore authStore, IProductsRepository productsRepository)
        {
            this.filesRepository = filesRepository;
            this.productsRepository = productsRepository;

            var token = authStore.LoadToken();
            if (token != null)
            {
                IsEditor = token.IsEditor();
                IsAdmin = token.IsAdmin();
            }

            deleteCommand = new DelegateCommand(PromtDelete, HasSelectedItems);
            deleteRequest = new InteractionRequest<Confirmation>();
        }

        public bool IsEditor { get; private set; }
        public bool IsAdmin { get; private set; }
        public ICommand DeleteCommand { get { return deleteCommand; } }
        public IInteractionRequest DeleteRequest { get { return deleteRequest; } }

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
            IsBusy = true;

            var task = await filesRepository.GetAll();

            if (task.Succeed)
            {
                var ids = task.Result.SelectMany(x => x.Metadata.ProductIds).Distinct().ToList();

                if (ids.Count > 0)
                {
                    var task2 = await productsRepository.GetNamesAsync(ids);
                    if (task2.Succeed)
                    {
                        names = task2.Result.ToDictionary(x => x.Id, x => x.Name);
                    }
                }

                PopulateNames(task.Result);

                Items = task.Result;
            }

            IsBusy = false;
        }

        public void OnNavigatedFrom()
        {
        }

        private bool HasSelectedItems()
        {
            return selectedItems != null && selectedItems.OfType<FileDescription>().Any();
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

        private void Delete(Confirmation conf)
        {
            if (conf.Confirmed)
            {
                //var ids = selectedItems.OfType<Product>().Select(x => x.Id).ToList();
                //var task = await productsRepository.Delete(ids);
                //if (task.Succeed)
                //{
                //    var args = new ProductDeletedBatchEventArgs(ids, false);
                //    eventAggregator.GetEvent<ProductDeletedBatchEvent>().Publish(args);
                //}
            }
        }

        #endregion

        private void PopulateNames(IEnumerable<FileDescription> files)
        {
            if (names != null && files != null)
            {
                foreach (var x in files)
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
}
