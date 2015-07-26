using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Win32;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class CreateFileViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private BitmapImage imageSource;
        private object[] selectedProducts;
        private bool isBusy;
        private OpenFileDialog dialog;
        private readonly InteractionRequest<ProductPickerViewModel> addProductRequest;
        private readonly DelegateCommand deleteProductCommand;
        private readonly Func<ProductPickerViewModel> pickerFactory;
        private readonly IFilesRepository filesRepository;

        public CreateFileViewModel(IFilesRepository filesRepository, Func<ProductPickerViewModel> pickerFactory)
        {
            this.filesRepository = filesRepository;
            this.pickerFactory = pickerFactory;

            addProductRequest = new InteractionRequest<ProductPickerViewModel>();
            Products = new ObservableCollection<ProductName>();
            AddProductCommand = new DelegateCommand(AddProduct);
            deleteProductCommand = new DelegateCommand(DeleteProduct, CanDeleteProduct);
            CancelCommand = new DelegateCommand(Close);
            SaveCommand = new DelegateCommand(Save);
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
        public ObservableCollection<ProductName> Products { get; private set; }
        public IInteractionRequest AddProductRequest { get { return addProductRequest; } }

        public BitmapImage ImageSource
        {
            get { return imageSource; }
            set { SetProperty(ref imageSource, value); }
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

        public void Init(OpenFileDialog d)
        {
            dialog = d;
            ImageSource = new BitmapImage(new Uri(dialog.FileName));
            Title = dialog.SafeFileName + "*";
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

        private void Close()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
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
                        Confirmed = true;
                        Close();
                    }
                }

                IsBusy = false;
            }
        }
    }
}
