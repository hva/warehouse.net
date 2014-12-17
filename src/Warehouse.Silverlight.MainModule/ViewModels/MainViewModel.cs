using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Data;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.Models;
using Warehouse.Silverlight.SignalR;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class MainViewModel : NotificationObject, INavigationAware
    {
        private readonly IDataService service;
        private readonly IEventAggregator eventAggregator;
        private readonly ISignalRClient signalRClient;
        private readonly InteractionRequest<ProductEditViewModel> editProductRequest;
        private readonly ICommand openProductCommand;
        private readonly CollectionViewSource cvs;
        private readonly ObservableCollection<Product> items;


        public MainViewModel(IDataService service, IEventAggregator eventAggregator, ISignalRClient signalRClient)
        {
            this.service = service;
            this.eventAggregator = eventAggregator;
            this.signalRClient = signalRClient;

            editProductRequest = new InteractionRequest<ProductEditViewModel>();
            openProductCommand = new DelegateCommand<Product>(OpenProduct);

            cvs = new CollectionViewSource();
            items = new ObservableCollection<Product>();
            cvs.Source = items;
            cvs.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            cvs.SortDescriptions.Add(new SortDescription("Size", ListSortDirection.Ascending));
        }

        public ICollectionView Items { get { return cvs.View; } }

        public ICommand OpenProductCommand { get { return openProductCommand; } }
        public IInteractionRequest EditProductRequest { get { return editProductRequest; } }

        public async void OnProductUpdated(ProductUpdatedEventArgs e)
        {
            var task = await service.GetProductAsync(e.ProductId);
            if (task.Succeed)
            {
                var current = task.Result;
                var old = items.FirstOrDefault(x => x.Id == current.Id);
                if (old != null)
                {
                    var index = items.IndexOf(old);
                    items.RemoveAt(index);
                    items.Insert(index, current);
                }
            }
        }

        private void Subscribe()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Subscribe(OnProductUpdated);
        }

        private void Unsubscribe()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Unsubscribe(OnProductUpdated);
        }

        private async void LoadData()
        {
            items.Clear();
            var task = await service.GetProductsAsync();
            if (task.Succeed)
            {
                items.AddRange(task.Result);
            }
        }

        private void OpenProduct(Product p)
        {
            editProductRequest.Raise(new ProductEditViewModel(p, service, eventAggregator));
        }

        #region INavigationAware

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadData();
            Subscribe();

            await signalRClient.EnsureConnection();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Unsubscribe();
        }

        #endregion
    }
}
