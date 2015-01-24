using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.Data.Products;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ChangePriceViewModel : InteractionRequestValidationObject
    {
        private string percentage = "10";

        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        public ChangePriceViewModel(IEnumerable<Product> products, IProductsRepository repository, IEventAggregator eventAggregator)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;

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
                    ValidatePercentage();
                    UpdatePrice();
                }
            }
        }

        private void ValidatePercentage()
        {
            errorsContainer.ClearErrors(() => Percentage);
            errorsContainer.SetErrors(() => Percentage, Validate.Double(Percentage));
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
            ValidatePercentage();
            if (HasErrors) return;

            var data = Items.Select(x => new ProductPriceUpdate { Id = x.Id, NewPrice = x.NewPrice }).ToArray();
            var task = await repository.UpdatePrice(data);
            if (task.Succeed)
            {
                foreach (var x in Items)
                {
                    var args = new ProductUpdatedEventArgs(x.Id, false);
                    eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(args);
                }

                Confirmed = task.Succeed;
                window.Close();
            }
        }
    }
}
