using System.Linq;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.Files.Converters;

namespace Warehouse.Wpf.Module.Files
{
    public class FilesViewModel : BindableBase, INavigationAware
    {
        private FileDescription[] items;
        private bool isBusy;
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
        }

        public bool IsEditor { get; private set; }
        public bool IsAdmin { get; private set; }

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

        public async void OnNavigatedTo(object param)
        {
            IsBusy = true;

            var task = await filesRepository.GetAll();

            if (task.Succeed)
            {
                var ids = task.Result.SelectMany(x => x.Metadata.ProductIds).Distinct().ToList();

                if (ids.Count > 0)
                {
                    var task2 = await productsRepository.GetManyAsync(ids);
                    if (task2.Succeed)
                    {
                        ProductIdToNameConverter.SetNames(task2.Result.ToDictionary(x => x.Id, x => string.Concat(x.Name, " ", x.Size)));
                    }
                }

                Items = task.Result;
            }

            IsBusy = false;
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
