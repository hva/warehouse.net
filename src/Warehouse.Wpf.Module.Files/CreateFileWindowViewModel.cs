using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class CreateFileWindowViewModel : BindableBase, INavigationAware
    {
        private BitmapImage imageSource;
        private string shortName;
        private string fullName;
        private string title;
        private object[] selectedProducts;
        private readonly InteractionRequest<ProductPickerViewModel> addProductRequest;
        private readonly DelegateCommand deleteProductCommand;
        private readonly Func<ProductPickerViewModel> pickerFactory;

        public CreateFileWindowViewModel(Func<ProductPickerViewModel> pickerFactory)
        {
            this.pickerFactory = pickerFactory;

            Products = new ObservableCollection<ProductName>();
            addProductRequest = new InteractionRequest<ProductPickerViewModel>();
            AddProductCommand = new DelegateCommand(AddProduct);
            deleteProductCommand = new DelegateCommand(DeleteProduct, CanDeleteProduct);
        }

        public IInteractionRequest AddProductRequest { get { return addProductRequest; } }
        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get { return deleteProductCommand; } }
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
            var dialog = param as OpenFileDialog;
            if (dialog != null)
            {
                shortName = dialog.SafeFileName;
                fullName = dialog.FileName;
                ImageSource = new BitmapImage(new Uri(fullName));
                Title = shortName + "*";
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
    }
}
