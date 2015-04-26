using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ChangePriceViewModel : InteractionRequestValidationObject
    {
        private string percentage = "10";
        private bool isBusy;

        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        public ChangePriceViewModel(IEnumerable<Product> products, IProductsRepository repository, IEventAggregator eventAggregator)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(() => IsWindowOpen = false);

            LoadItems(products);
            UpdatePrice();
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }
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

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
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
                select new ChangePriceItem(p);

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

        private async void Save()
        {
            ValidatePercentage();
            if (HasErrors) return;

            var data = Items.Select(x => new ProductPriceUpdate
            {
                Id = x.Product.Id,
                NewPriceOpt = x.NewPriceOpt,
                NewPriceRozn = x.NewPriceRozn
            }).ToArray();

            IsBusy = true;
            var task = await repository.UpdatePrice(data);
            IsBusy = false;
            if (task.Succeed)
            {
                var ids = Items.Select(x => x.Product.Id).ToList();
                var args = new ProductUpdatedBatchEventArgs(ids, false);
                eventAggregator.GetEvent<ProductUpdatedBatchEvent>().Publish(args);

                Confirmed = task.Succeed;
                IsWindowOpen = false;
            }
        }
    }
}
