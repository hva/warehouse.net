using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.UI.Modules.Files
{
    public class ProductPickerViewModel : DialogViewModel
    {
        private readonly IProductsRepository productsRepository;
        private string searchQuery;
        private readonly CollectionViewSource cvs;
        private readonly ObservableCollection<ProductName> items;

        public ProductPickerViewModel(IProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;

            items = new ObservableCollection<ProductName>();
            cvs = new CollectionViewSource { Source = items };
            cvs.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

            Title = "Товарные позиции";
        }

        public object[] SelectedItems { get; set; }
        public ProductName[] SelectedProducts { get; private set; }
        public ICollectionView Items => cvs.View;

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

        public async Task InitAsync(IEnumerable<ProductName> excluded)
        {
            var task = await productsRepository.GetNamesAsync();
            if (task.Succeed)
            {
                items.Clear();
                var included = task.Result.Except(excluded, new ProductNameComparer());
                items.AddRange2(included);
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

        protected override Task SaveAsync()
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

            return Task.FromResult<object>(null);
        }
    }
}
