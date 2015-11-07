using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Prism.Mvvm;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.UI.Modules.Products.Picker
{
    public class ProductsPickerViewModel : BindableBase
    {
        private bool isBusy;
        private string searchQuery;
        private object[] selectedItems;
        private readonly CollectionViewSource cvs;
        private readonly ObservableCollection<ProductName> items;
        private readonly IProductsRepository productsRepository;

        public ProductsPickerViewModel(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;

            items = new ObservableCollection<ProductName>();
            cvs = new CollectionViewSource { Source = items };
            cvs.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
        }

        public ICollectionView Items => cvs.View;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public SelectionMode SelectionMode { get; private set; }
        public Action<ProductName[]> SelectionCallback { get; private set; }

        public object[] SelectedItems
        {
            get { return selectedItems; }
            set
            {
                selectedItems = value;
                if (SelectionCallback != null && selectedItems != null)
                {
                    var products = selectedItems.OfType<ProductName>().ToArray();
                    if (products.Length > 0)
                    {
                        SelectionCallback(products);
                    }
                }
            }
        }

        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                if (searchQuery != value)
                {
                    searchQuery = value;

                    cvs.Filter -= OnFilter;
                    if (!string.IsNullOrEmpty(SearchQuery))
                    {
                        cvs.Filter += OnFilter;
                    }
                    cvs.View.Refresh();
                }
            }
        }

        public async Task InitAsync(SelectionMode selectionMode, Action<ProductName[]> selectionCallback)
        {
            SelectionMode = selectionMode;
            SelectionCallback = selectionCallback;

            IsBusy = true;
            var task = await productsRepository.GetNamesAsync();
            IsBusy = false;

            if (task.Succeed)
            {
                foreach (var x in task.Result)
                {
                    items.Add(x);
                }
            }
        }

        private void OnFilter(object sender, FilterEventArgs e)
        {
            var item = e.Item as ProductName;
            if (item != null)
            {
                if (!item.Name.ContainsIgnoreKey(searchQuery))
                {
                    e.Accepted = false;
                }
            }
        }

    }
}
