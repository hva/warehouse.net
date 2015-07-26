using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class ProductPickerViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private readonly IProductsRepository productsRepository;
        private ProductName[] items;

        public ProductPickerViewModel(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;

            CancelCommand = new DelegateCommand(Close);
            PickCommand = new DelegateCommand(Pick);
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand PickCommand { get; private set; }
        public object[] SelectedItems { get; set; }
        public ProductName[] SelectedProducts { get; private set; }

        public ProductName[] Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

        #region IInteractionRequestAware
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
        #endregion

        #region IConfirmation
        public string Title { get; set; }
        public object Content { get; set; }
        public bool Confirmed { get; set; }
        #endregion

        public async Task InitAsync()
        {
            var task = await productsRepository.GetNamesAsync();
            if (task.Succeed)
            {
                Items = task.Result.ToArray();
            }
        }

        private void Close()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }

        private void Pick()
        {
            if (SelectedItems != null)
            {
                SelectedProducts = SelectedItems.OfType<ProductName>().ToArray();
                if (SelectedProducts.Length > 0)
                {
                    Confirmed = true;
                }
            }
            Close();
        }
    }
}
