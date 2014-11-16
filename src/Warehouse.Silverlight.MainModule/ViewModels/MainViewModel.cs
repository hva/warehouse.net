using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class MainViewModel : NotificationObject, INavigationAware
    {
        private readonly IDataService service;
        private readonly IEventAggregator eventAggregator;
        private readonly InteractionRequest<ProductEditViewModel> editProductRequest;
        private readonly ICommand openProductCommand;
        private ObservableCollection<Product> items;

        public MainViewModel(IDataService service, IEventAggregator eventAggregator)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;

            editProductRequest = new InteractionRequest<ProductEditViewModel>();
            openProductCommand = new DelegateCommand<Product>(OpenProduct);

            Subscribe();

            LoadData();
        }

        public ObservableCollection<Product> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(() => Items); }
        }

        public ICommand OpenProductCommand { get { return openProductCommand; } }
        public IInteractionRequest EditProductRequest { get { return editProductRequest; } }

        public async void OnProductUpdated(ProductUpdatedEventArgs e)
        {
            var task = await service.GetProductAsync(e.ProductId);
            if (task.Succeed)
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

        private void Subscribe()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Subscribe(OnProductUpdated);
        }

        private async void LoadData()
        {
            var task = await service.GetProductsAsync();
            if (task.Succeed)
            {
                Items = new ObservableCollection<Product>(task.Result);
            }
        }

        private void OpenProduct(Product p)
        {
            editProductRequest.Raise(new ProductEditViewModel(p, service, eventAggregator));
        }

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        #endregion
    }
}
