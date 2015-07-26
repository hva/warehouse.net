using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Files
{
    public class ProductPickerViewModel : BindableBase, IConfirmation, IInteractionRequestAware
    {
        private readonly IProductsRepository productsRepository;
        private string searchQuery;
        private readonly CollectionViewSource cvs;
        private readonly ObservableCollection<ProductName> items;

        public ProductPickerViewModel(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;

            CancelCommand = new DelegateCommand(Close);
            PickCommand = new DelegateCommand(Pick);

            items = new ObservableCollection<ProductName>();
            cvs = new CollectionViewSource { Source = items };
            cvs.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public ICommand CancelCommand { get; private set; }
        public ICommand PickCommand { get; private set; }
        public object[] SelectedItems { get; set; }
        public ProductName[] SelectedProducts { get; private set; }
        public ICollectionView Items { get { return cvs.View; } }

        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;
                    cvs.View.Refresh();
                }
            }
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
                items.Clear();
                items.AddRange(task.Result);

                cvs.Filter -= OnFilter;
                cvs.Filter += OnFilter;
            }
        }

        private void OnFilter(object sender, FilterEventArgs e)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return;
            }

            var item = e.Item as ProductName;
            if (item != null)
            {
                e.Accepted = item.Name.IndexOf(searchQuery, StringComparison.InvariantCultureIgnoreCase) >= 0;
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
