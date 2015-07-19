using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.PubSubEvents;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Events;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Mvvm;

namespace Warehouse.Wpf.Module.ChangePrice
{
    public class ChangePriceWindowViewModel : ValidationObject, INavigationAware
    {
        private string percentage = "10";
        private bool isBusy;
        private bool isWindowOpen = true;
        private ChangePriceItem[] items;

        private readonly IProductsRepository repository;
        private readonly IEventAggregator eventAggregator;

        public ChangePriceWindowViewModel(IProductsRepository repository, IEventAggregator eventAggregator)
        {
            this.repository = repository;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(() => IsWindowOpen = false);
        }

        public ICommand SaveCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ChangePriceItem[] Items
        {
            get { return items; }
            set { SetProperty(ref items, value); }
        }

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
            set { SetProperty(ref isBusy, value); }
        }

        public bool IsWindowOpen
        {
            get { return isWindowOpen; }
            set { SetProperty(ref isWindowOpen, value); }
        }

        private void ValidatePercentage()
        {
            errorsContainer.ClearErrors(() => Percentage);
            errorsContainer.SetErrors(() => Percentage, Validate.Double(Percentage));
        }

        private void LoadItems(IEnumerable<Product> products)
        {
            var q =
                from p in products
                select new ChangePriceItem(p);

            Items = q.ToArray();
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
                IsWindowOpen = false;
            }
        }

        public void OnNavigatedTo(object param)
        {
            var products = param as IEnumerable<Product>;
            if (products != null)
            {
                LoadItems(products);
                UpdatePrice();
            }
        }

        public void OnNavigatedFrom()
        {
            
        }
    }
}
