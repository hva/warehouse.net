using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class MainViewModel : NotificationObject // : INavigationAware 
    {
        private readonly IDataService service;
        private readonly InteractionRequest<ProductEditViewModel> editProductRequest;
        private readonly ICommand openProductCommand;
        private ObservableCollection<Product> items;

        public MainViewModel(IDataService service)
        {
            this.service = service;

            editProductRequest = new InteractionRequest<ProductEditViewModel>();
            openProductCommand = new DelegateCommand<Product>(OpenProduct);

            LoadData();
        }

        public ObservableCollection<Product> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand OpenProductCommand { get { return openProductCommand; } }
        public IInteractionRequest EditProductRequest { get { return editProductRequest; } }

        private async void LoadData()
        {
            var hubConnection = new HubConnection(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString());
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("ProductsHub");
            stockTickerHubProxy.On<string>("UpdateStockPrice", stock =>
            {

            });
            await hubConnection.Start();

            var task = await service.GetProductsAsync();
            if (task.Success)
            {
                Items = new ObservableCollection<Product>(task.Result);
            }
        }

        private void OpenProduct(Product p)
        {
            editProductRequest.Raise(new ProductEditViewModel(p, service), OnEditProductClosed);
        }

        private async void OnEditProductClosed(ProductEditViewModel vm)
        {
            if (vm.Confirmed)
            {
                var task = await service.GetProductAsync(vm.Id);
                if (task.Success)
                {
                    var current = task.Result;
                    var old = Items.FirstOrDefault(x => x.Id == current.Id);
                    if (old != null)
                    {
                        var index = Items.IndexOf(old);
                        Items.RemoveAt(index);
                        Items.Insert(index, current);
                    }
                }
            }
        }
    }
}
