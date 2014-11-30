using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.Data;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.MainModule.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestValidationObject
    {
        private readonly IDataService dataService;
        private readonly IEventAggregator eventAggregator;

        private string id;
        private string name;
        private string size;
        private string k;
        private string priceOpt;
        private string priceRozn;
        private string weight;
        private string count;

        public ProductEditViewModel(Product product, IDataService dataService, IEventAggregator eventAggregator)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;

            Title = string.Format("{0} {1}", product.Name, product.Size);
            SaveCommand = new DelegateCommand<ChildWindow>(Save);

            ProductToProps(product);
        }

        public ICommand SaveCommand { get; private set; }

        #region Name

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    ValidateName();
                }
            }
        }

        private void ValidateName()
        {
            errorsContainer.ClearErrors(() => Name);
            errorsContainer.SetErrors(() => Name, Validate.Required(Name));
        }

        #endregion

        #region Size

        public string Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    ValidateSize();
                }
            }
        }

        private void ValidateSize()
        {
            errorsContainer.ClearErrors(() => Size);
            errorsContainer.SetErrors(() => Size, Validate.Required(Size));
        }

        #endregion

        #region K

        public string K
        {
            get { return k; }
            set { k = value; ValidateK(); }
        }

        private void ValidateK()
        {
            errorsContainer.ClearErrors(() => K);
            errorsContainer.SetErrors(() => K, Validate.Double(K));
        }

        #endregion

        #region PriceOpt

        public string PriceOpt
        {
            get { return priceOpt; }
            set { priceOpt = value; ValidatePriceOpt(); }
        }

        private void ValidatePriceOpt()
        {
            errorsContainer.ClearErrors(() => PriceOpt);
            errorsContainer.SetErrors(() => PriceOpt, Validate.Long(PriceOpt));
        }

        #endregion

        #region PriceRozn

        public string PriceRozn
        {
            get { return priceRozn; }
            set { priceRozn = value; ValidatePriceRozn(); }
        }

        private void ValidatePriceRozn()
        {
            errorsContainer.ClearErrors(() => PriceRozn);
            errorsContainer.SetErrors(() => PriceRozn, Validate.Long(PriceRozn));
        }

        #endregion

        #region Weight

        public string Weight
        {
            get { return weight; }
            set { weight = value; ValidateWeight(); }
        }

        private void ValidateWeight()
        {
            errorsContainer.ClearErrors(() => Weight);
            errorsContainer.SetErrors(() => Weight, Validate.Double(Weight));
        }

        #endregion

        #region Count

        public string Count
        {
            get { return count; }
            set { count = value; ValidateCount(); }
        }

        private void ValidateCount()
        {
            errorsContainer.ClearErrors(() => Count);
            errorsContainer.SetErrors(() => Count, Validate.Int(Count));
        }

        #endregion

        private async void Save(ChildWindow window)
        {
            ValidateName();
            ValidateSize();
            ValidateK();
            ValidatePriceOpt();
            ValidatePriceRozn();
            ValidateWeight();
            ValidateCount();

            if (HasErrors) return;

            var changed = PropsToProduct();

            var task = await dataService.SaveProductAsync(changed);
            if (task.Succeed)
            {
                eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(new ProductUpdatedEventArgs(id));
                Confirmed = true;
                window.Close();
            }
        }

        private void ProductToProps(Product product)
        {
            id = product.Id;
            name = product.Name;
            size = product.Size;
            k = product.K.ToString("0.##");
            priceOpt = product.PriceOpt.ToString(CultureInfo.InvariantCulture);
            priceRozn = product.PriceRozn.ToString(CultureInfo.InvariantCulture);
            weight = product.Weight.ToString("0");
            count = product.Count.ToString(CultureInfo.InvariantCulture);
        }

        private Product PropsToProduct()
        {
            return new Product
            {
                Id = id,
                Name = name,
                Size = size,
                K = Math.Round(double.Parse(k), 2),
                PriceOpt = long.Parse(priceOpt),
                PriceRozn = long.Parse(priceRozn),
                Weight = double.Parse(weight),
                Count = int.Parse(count),
            };
        }
    }
}
