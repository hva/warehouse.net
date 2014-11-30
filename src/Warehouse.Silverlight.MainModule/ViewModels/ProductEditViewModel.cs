using System;
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

        private async void Save(ChildWindow window)
        {
            ValidateName();
            ValidateSize();
            ValidateK();

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
        }

        private Product PropsToProduct()
        {
            return new Product
            {
                Id = id,
                Name = name,
                Size = size,
                K = Math.Round(double.Parse(k), 2),
            };
        }
    }
}
