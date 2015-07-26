using System;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
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
        private ProductName[] products;
        private readonly InteractionRequest<ProductPickerViewModel> addProductRequest;
        private readonly Func<ProductPickerViewModel> pickerFactory;

        public CreateFileWindowViewModel(Func<ProductPickerViewModel> pickerFactory)
        {
            this.pickerFactory = pickerFactory;

            addProductRequest = new InteractionRequest<ProductPickerViewModel>();
            AddProductCommand = new DelegateCommand(AddProduct);
        }

        public IInteractionRequest AddProductRequest { get { return addProductRequest; } }
        public ICommand AddProductCommand { get; private set; }

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

        public ProductName[] Products
        {
            get { return products; }
            set { SetProperty(ref products, value); }
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
            await context.InitAsync();
            addProductRequest.Raise(context, x =>
            {
                if (x.Confirmed)
                {
                    Products = x.SelectedProducts;
                }
            });
        }
    }
}
