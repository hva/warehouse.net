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
        private bool isBusy;
        private readonly IFilesRepository filesRepository;

        public FilesViewModel(IFilesRepository filesRepository, IAuthStore authStore)
        {
            this.filesRepository = filesRepository;

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
            IsBusy = false;

            if (task.Succeed)
            {
                Items = task.Result;
            }
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
