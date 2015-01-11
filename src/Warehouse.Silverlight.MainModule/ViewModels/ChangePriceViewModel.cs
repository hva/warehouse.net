using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Data.Products;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ChangePriceViewModel : InteractionRequestObject
    {
        private readonly IProductsRepository repository;
        private string percentage = "10";

        public ChangePriceViewModel(IEnumerable<Product> products, IProductsRepository repository)
        {
            this.repository = repository;

            SaveCommand = new DelegateCommand<ChildWindow>(Save);

            LoadItems(products);
            UpdatePrice();
        }

        public ICommand SaveCommand { get; private set; }
        public ObservableCollection<ChangePriceItem> Items { get; private set; }

        public string Percentage
        {
            get { return percentage; }
            set
            {
                if (percentage != value)
                {
                    percentage = value;
                    UpdatePrice();
                }
            }
        }

        private void LoadItems(IEnumerable<Product> products)
        {
            var items =
                from p in products
                select new ChangePriceItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Size = p.Size,
                    PriceOpt = p.PriceOpt,
                };

            Items = new ObservableCollection<ChangePriceItem>(items);
        }

        private void UpdatePrice()
        {
            double p;
            double.TryParse(percentage, out p);
            foreach (var x in Items)
            {
                x.Refresh(p);
            }
        }

        private async void Save(ChildWindow window)
        {
            var data = Items.Select(x => new ProductPriceUpdate { Id = x.Id, NewPrice = x.NewPrice }).ToArray();
            var task = await repository.UpdatePrice(data);
            // TODO: check for error
            Confirmed = task.Succeed;
            window.Close();
        }
    }
}
