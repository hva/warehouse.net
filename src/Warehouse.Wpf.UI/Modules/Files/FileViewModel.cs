using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.UI.Modules.Files
{
    public abstract class FileViewModel : DialogViewModel
    {
        private Uri uri;
        private object[] selectedProducts;
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
            PrintCommand = new DelegateCommand(Print);
        }

        public ICommand AddProductCommand { get; private set; }
        public ICommand DeleteProductCommand => deleteProductCommand;
        public ICommand PrintCommand { get; private set; }
        public ObservableCollection<ProductName> Products { get; }
        public IInteractionRequest AddProductRequest => addProductRequest;

        public Uri Uri
        {
            get { return uri; }
            set { SetProperty(ref uri, value); }
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

        private async void AddProduct()
        {
            var context = pickerFactory();
            await context.InitAsync(Products);

            addProductRequest.Raise(context, x =>
            {
                if (x.Confirmed)
                {
                    Products.AddRange2(x.SelectedProducts);
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
