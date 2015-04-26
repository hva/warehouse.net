using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.Models;
using Warehouse.Silverlight.SignalR;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class MainViewModel : NotificationObject, INavigationAware, IRegionMemberLifetime
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ISignalRClient signalRClient;
        private readonly IProductsRepository productsRepository;
        private readonly ProductEditWindowViewModel productEditViewModel;
        private readonly ProductCreateWindowViewModel productCreateViewModel;
        private readonly InteractionRequest<ProductCreateWindowViewModel> createProductRequest;
        private readonly InteractionRequest<ProductEditWindowViewModel> editProductRequest;
        private readonly InteractionRequest<ChangePriceViewModel> changePriceRequest;
        private readonly InteractionRequest<Confirmation> deleteRequest;
        private readonly CollectionViewSource cvs;
        private readonly ObservableCollection<Product> items;
        private IList selectedItems;
        private readonly DelegateCommand changePriceCommand;
        private readonly DelegateCommand deleteCommand;
        private double totalWeight;

        public MainViewModel(IEventAggregator eventAggregator, ISignalRClient signalRClient, IAuthStore authStore, IProductsRepository productsRepository,
            ProductEditWindowViewModel productEditViewModel, ProductCreateWindowViewModel productCreateViewModel)
        {
            this.eventAggregator = eventAggregator;
            this.signalRClient = signalRClient;
            this.productsRepository = productsRepository;
            this.productEditViewModel = productEditViewModel;
            this.productCreateViewModel = productCreateViewModel;

            createProductRequest = new InteractionRequest<ProductCreateWindowViewModel>();
            editProductRequest = new InteractionRequest<ProductEditWindowViewModel>();
            changePriceRequest = new InteractionRequest<ChangePriceViewModel>();
            deleteRequest = new InteractionRequest<Confirmation>();
            OpenProductCommand = new DelegateCommand<Product>(OpenProduct);
            CreateProductCommand = new DelegateCommand(CreateProduct);
            changePriceCommand = new DelegateCommand(ChangePrice, HasSelectedProducts);
            deleteCommand = new DelegateCommand(PromtDelete, HasSelectedProducts);

            cvs = new CollectionViewSource();
            items = new ObservableCollection<Product>();
            cvs.Source = items;
            cvs.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            cvs.SortDescriptions.Add(new SortDescription("Size", ListSortDirection.Ascending));

            var token = authStore.LoadToken();
            if (token != null)
            {
                IsEditor = token.IsEditor();
                IsAdmin = token.IsAdmin();
            }
        }

        public ICollectionView Items { get { return cvs.View; } }

        public ICommand OpenProductCommand { get; private set; }
        public ICommand CreateProductCommand { get; private set; }
        public ICommand ChangePriceCommand { get { return changePriceCommand; } }
        public ICommand DeleteCommand { get { return deleteCommand; } }
        public IInteractionRequest CreateProductRequest { get { return createProductRequest; } }
        public IInteractionRequest EditProductRequest { get { return editProductRequest; } }
        public IInteractionRequest ChangePriceRequest { get { return changePriceRequest; } }
        public IInteractionRequest DeleteRequest { get { return deleteRequest; } }
        public bool IsEditor { get; private set; }
        public bool IsAdmin { get; private set; }

        public IList SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                changePriceCommand.RaiseCanExecuteChanged();
                deleteCommand.RaiseCanExecuteChanged();
            }
        }

        public double TotalWeight
        {
            get { return totalWeight; }
            set { totalWeight = value; RaisePropertyChanged(() => TotalWeight); }
        }

        #region EventHandlers

        public async void OnProductUpdated(ProductUpdatedEventArgs e)
        {
            var task = await productsRepository.GetAsync(e.ProductId);
            if (task.Succeed)
            {
                UpdateProductItem(task.Result);
                UpdateTotalWeight();
            }
        }

        private void UpdateProductItem(Product current)
        {
            if (items == null) return;

            var old = items.FirstOrDefault(x => x.Id == current.Id);
            if (old != null)
            {
                var index = items.IndexOf(old);
                items.RemoveAt(index);
                items.Insert(index, current);
            }
            else
            {
                items.Add(current);
            }
        }

        public async void OnProductsUpdated(ProductUpdatedBatchEventArgs args)
        {
            if (args == null || args.ProductIds == null) return;

            var task = await productsRepository.GetManyAsync(args.ProductIds);
            if (task.Succeed)
            {
                foreach (var product in task.Result)
                {
                    UpdateProductItem(product);
                }
                UpdateTotalWeight();
            }
        }

        public void OnProductsDeleted(ProductDeletedBatchEventArgs args)
        {
            if (items == null) return;
            if (args == null || args.ProductIds == null) return;

            foreach (var id in args.ProductIds)
            {
                string _id = id;
                var p = items.FirstOrDefault(x => x.Id == _id);
                if (p != null)
                {
                    items.Remove(p);
                }
            }
            UpdateTotalWeight();
        }

        private void Subscribe()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Subscribe(OnProductUpdated);
            eventAggregator.GetEvent<ProductUpdatedBatchEvent>().Subscribe(OnProductsUpdated);
            eventAggregator.GetEvent<ProductDeletedBatchEvent>().Subscribe(OnProductsDeleted);
        }


        private void Unsubscribe()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Unsubscribe(OnProductUpdated);
            eventAggregator.GetEvent<ProductUpdatedBatchEvent>().Unsubscribe(OnProductsUpdated);
            eventAggregator.GetEvent<ProductDeletedBatchEvent>().Unsubscribe(OnProductsDeleted);
        }

        #endregion

        private async Task LoadData()
        {
            var task = await productsRepository.GetAsync();
            if (task.Succeed)
            {
                items.AddRange(task.Result);
                UpdateTotalWeight();
            }
        }

        private void OpenProduct(Product p)
        {
            editProductRequest.Raise(productEditViewModel.Init(p));
        }

        private void CreateProduct()
        {
            createProductRequest.Raise(productCreateViewModel.Init());
        }

        private void UpdateTotalWeight()
        {
            if (items != null)
            {
                TotalWeight = items.Sum(x => x.Weight) / 1000;
            }
        }

        private bool HasSelectedProducts()
        {
            return selectedItems != null && selectedItems.OfType<Product>().Any();
        }

        #region ChangePrice

        private void ChangePrice()
        {
            var products = selectedItems.OfType<Product>().ToArray();
            changePriceRequest.Raise(new ChangePriceViewModel(products, productsRepository, eventAggregator));
        }

        #endregion

        #region Delete

        private void PromtDelete()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Следующие позиции будут удалены:");
            foreach (var x in selectedItems.OfType<Product>())
            {
                sb.AppendFormat("- {0} {1}", x.Name, x.Size);
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
                var ids = selectedItems.OfType<Product>().Select(x => x.Id).ToList();
                var task = await productsRepository.Delete(ids);
                if (task.Succeed)
                {
                    var args = new ProductDeletedBatchEventArgs(ids, false);
                    eventAggregator.GetEvent<ProductDeletedBatchEvent>().Publish(args);
                }
            }
        }

        #endregion

        #region INavigationAware

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            await LoadData();
            Subscribe();

            await signalRClient.EnsureConnection();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Unsubscribe();
        }

        #endregion

        #region IRegionMemberLifetime

        public bool KeepAlive { get { return false; } }

        #endregion
    }
}
