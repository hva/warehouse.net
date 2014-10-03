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
        private readonly InteractionRequest<Notification> notificationRequst = new InteractionRequest<Notification>();

        public MainViewModel(IDataService service)
        {
            this.service = service;

            OpenProductCommand = new DelegateCommand<Product>(OpenProduct);

            LoadData();
        }

        public Product[] Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand OpenProductCommand { get; private set; }

        public IInteractionRequest NotificationRequst { get { return notificationRequst; } }

        private async void LoadData()
        {
            Items = await service.GetProductsAsync();
        }

        private void OpenProduct(Product p)
        {
            notificationRequst.Raise(
                new Notification {Title = "title", Content = "content"},
                x =>
                {
                    
                }
            );
        }
    }
}
