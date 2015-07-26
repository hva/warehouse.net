using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Win32;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class CreateFileWindowViewModel : BindableBase, INavigationAware
    {
        private BitmapImage imageSource;
        private string title;
        private object[] selectedProducts;
        private bool isWindowOpen = true;
        private bool isBusy;
        private OpenFileDialog dialog;
        private readonly InteractionRequest<ProductPickerViewModel> addProductRequest;
        private readonly DelegateCommand deleteProductCommand;
        private readonly Func<ProductPickerViewModel> pickerFactory;
        private readonly IFilesRepository filesRepository;
        private readonly IEventAggregator eventAggregator;

        public CreateFileWindowViewModel(IFilesRepository filesRepository, IEventAggregator eventAggregator,
            Func<ProductPickerViewModel> pickerFactory)
        {
            this.filesRepository = filesRepository;
            this.eventAggregator = eventAggregator;
            this.pickerFactory = pickerFactory;

            Products = new ObservableCollection<ProductName>();
            addProductRequest = new InteractionRequest<ProductPickerViewModel>();
            AddProductCommand = new DelegateCommand(AddProduct);
            deleteProductCommand = new DelegateCommand(DeleteProduct, CanDeleteProduct);
            CancelCommand = new DelegateCommand(Close);
            SaveCommand = new DelegateCommand(Save);
        }

        public IInteractionRequest AddProductRequest { get { return addProductRequest; } }
        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get { return deleteProductCommand; } }
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ObservableCollection<ProductName> Products { get; private set; }

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set { SetProperty(ref isWindowOpen, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public object[] SelectedProducts
        {
            get { return selectedProducts; }
            set
            {
                selectedProducts = value;
                deleteProductCommand.RaiseCanExecuteChanged();
            }
        }

        public void OnNavigatedTo(object param)
        {
            dialog = param as OpenFileDialog;
            if (dialog != null)
            {
                ImageSource = new BitmapImage(new Uri(dialog.FileName));
                Title = dialog.SafeFileName + "*";
            }
        }

        public void OnNavigatedFrom()
        {
            
        }

        private async void AddProduct()
        {
            var context = pickerFactory();
            context.Title = "Товарные позиции";
            await context.InitAsync(Products);
            addProductRequest.Raise(context, x =>
            {
                if (x.Confirmed)
                {
                    Products.AddRange(x.SelectedProducts);
                }
            });
        }

        private void DeleteProduct()
        {
            foreach (var x in selectedProducts.OfType<ProductName>())
            {
                Products.Remove(x);
            }
        }

        private bool CanDeleteProduct()
        {
            return selectedProducts != null && selectedProducts.OfType<ProductName>().Any();
        }

        private void Close()
        {
            IsWindowOpen = false;
        }

        private async void Save()
        {
            if (dialog != null)
            {
                IsBusy = true;

                var task = await filesRepository.Create(dialog.OpenFile(), dialog.SafeFileName, "image/jpeg");
                if (task.Succeed)
                {
                    var fileId = task.Result;
                    var productIds = Products.Select(x => x.Id).ToArray();
                    var task2 = await filesRepository.AttachProducts(fileId, productIds);
                    if (task2.Succeed)
                    {
                        eventAggregator.GetEvent<FileUpdatedEvent>().Publish(null);
                        Close();
                    }
                }

                IsBusy = false;
            }
        }
    }
}
