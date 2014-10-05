using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class MainViewModel : NotificationObject // : INavigationAware 
    {
        private Product[] items;
        private readonly IDataService service;
        private readonly InteractionRequest<ProductEditViewModel> editProductRequest;
        private readonly ICommand openProductCommand;

        public MainViewModel(IDataService service)
        {
            this.service = service;

            editProductRequest = new InteractionRequest<ProductEditViewModel>();
            openProductCommand = new DelegateCommand<Product>(OpenProduct);

            LoadData();
        }

        public Product[] Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand OpenProductCommand { get { return openProductCommand; } }
        public IInteractionRequest EditProductRequest { get { return editProductRequest; } }

        private async void LoadData()
        {
            var task = await service.GetProductsAsync();
            if (task.Success)
            {
                Items = task.Result;
            }
        }

        private void OpenProduct(Product p)
        {
            editProductRequest.Raise(new ProductEditViewModel(p));
        }
    }
}
