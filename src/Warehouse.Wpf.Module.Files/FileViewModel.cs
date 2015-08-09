using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public abstract class FileViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private Uri uri;
        private object[] selectedProducts;
        private bool isBusy;
        private readonly InteractionRequest<ProductPickerViewModel> addProductRequest;
        private readonly DelegateCommand deleteProductCommand;
        private readonly Func<ProductPickerViewModel> pickerFactory;

        protected FileViewModel(Func<ProductPickerViewModel> pickerFactory)
        {
            this.pickerFactory = pickerFactory;

            addProductRequest = new InteractionRequest<ProductPickerViewModel>();
            Products = new ObservableCollection<ProductName>();
            AddProductCommand = new DelegateCommand(AddProduct);
            deleteProductCommand = new DelegateCommand(DeleteProduct, CanDeleteProduct);
            CancelCommand = new DelegateCommand(Close);
            SaveCommand = new DelegateCommand(Save);
            PrintCommand = new DelegateCommand(Print);
        }

        #region IConfirmation, IInteractionRequestAware
        public string Title { get; set; }
        public object Content { get; set; }
        public bool Confirmed { get; set; }
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
        #endregion

        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand { get { return deleteProductCommand; } }
        public ICommand CancelCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand PrintCommand { get; private set; }
        public ObservableCollection<ProductName> Products { get; private set; }
        public IInteractionRequest AddProductRequest { get { return addProductRequest; } }

        public Uri Uri
        {
            get { return uri; }
            set { SetProperty(ref uri, value); }
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

        protected abstract void Save();

        protected void Close()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }

        private async void AddProduct()
        {
            var context = pickerFactory();
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

        private void Print()
        {
            var bitmapImage = new BitmapImage(Uri);
            if (bitmapImage.IsDownloading)
            {
                bitmapImage.DownloadCompleted += (s, e) => PrintImage(bitmapImage);
            }
            else
            {
                PrintImage(bitmapImage);
            }
        }

        private void PrintImage(BitmapSource bitmapImage)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                var image = new Image { Source = bitmapImage };
                if (bitmapImage.PixelWidth > bitmapImage.PixelHeight)
                {
                    image.LayoutTransform = new RotateTransform(90);
                }

                var pageSize = new Size { Height = printDialog.PrintableAreaHeight, Width = printDialog.PrintableAreaWidth };
                image.Measure(pageSize);
                image.UpdateLayout();
                printDialog.PrintVisual(image, Title);
            }
        }
    }
}
