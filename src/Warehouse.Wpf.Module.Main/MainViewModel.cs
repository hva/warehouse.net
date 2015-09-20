﻿using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.SignalR.Interfaces;

namespace Warehouse.Wpf.Module.Main
{
    public class MainViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ISignalRClient signalRClient;
        private readonly IProductsRepository productsRepository;
        private readonly InteractionRequest<Confirmation> deleteRequest;
        private readonly CollectionViewSource cvs;
        private readonly ObservableCollection<Product> items;
        private IList selectedItems;
        private readonly DelegateCommand changePriceCommand;
        private readonly DelegateCommand deleteCommand;
        private double totalWeight;
        private bool isBusy;

        public MainViewModel(IEventAggregator eventAggregator, ISignalRClient signalRClient, IAuthStore authStore,
            IProductsRepository productsRepository)
        {
            this.eventAggregator = eventAggregator;
            this.signalRClient = signalRClient;
            this.productsRepository = productsRepository;

            deleteRequest = new InteractionRequest<Confirmation>();
            CreateProductCommand = new DelegateCommand(CreateProduct);
            OpenProductCommand = new DelegateCommand<Product>(EditProduct);
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
        //public IInteractionRequest ChangePriceRequest { get { return changePriceRequest; } }
        public IInteractionRequest DeleteRequest { get { return deleteRequest; } }
        public bool IsEditor { get; private set; }
        public bool IsAdmin { get; private set; }

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
                changePriceCommand.RaiseCanExecuteChanged();
                deleteCommand.RaiseCanExecuteChanged();
            }
        }

        public double TotalWeight
        {
            get { return totalWeight; }
            set { SetProperty(ref totalWeight, value); }
        }

        #region EventHandlers

        public async void OnProductUpdated(ProductUpdatedEventArgs e)
        {
            IsBusy = true;
            var task = await productsRepository.GetAsync(e.ProductId);
            IsBusy = false;
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

            IsBusy = true;
            var task = await productsRepository.GetManyAsync(args.ProductIds);
            IsBusy = false;
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
            IsBusy = true;
            var task = await productsRepository.GetAsync();
            IsBusy = false;
            if (task.Succeed)
            {
                items.AddRange(task.Result);
                UpdateTotalWeight();
            }
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

        private void CreateProduct()
        {
            var args = new OpenWindowEventArgs(PageName.ProductCreateWindow, null);
            eventAggregator.GetEvent<OpenWindowEvent>().Publish(args);
        }

        private void EditProduct(Product p)
        {
            var args = new OpenWindowEventArgs(PageName.ProductEditWindow, p);
            eventAggregator.GetEvent<OpenWindowEvent>().Publish(args);
        }

        private void ChangePrice()
        {
            var products = selectedItems.OfType<Product>().ToArray();
            var args = new OpenWindowEventArgs(PageName.ChangePriceWindow, products);
            eventAggregator.GetEvent<OpenWindowEvent>().Publish(args);
        }

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
                IsBusy = true;
                var task = await productsRepository.Delete(ids);
                IsBusy = false;
                if (task.Succeed)
                {
                    var args = new ProductDeletedBatchEventArgs(ids, false);
                    eventAggregator.GetEvent<ProductDeletedBatchEvent>().Publish(args);
                }
            }
        }

        #endregion

        #region INavigationAware

        public async void OnNavigatedTo(object param)
        {
            await LoadData();
            Subscribe();

            await signalRClient.EnsureConnection();
        }

        public void OnNavigatedFrom()
        {
            Unsubscribe();
        }

        #endregion
    }
}
