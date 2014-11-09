using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.Infrastructure.Events;
using Warehouse.Silverlight.MainModule.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestValidationObject
    {
        private readonly IDataService dataService;
        private readonly IEventAggregator eventAggregator;
        private readonly string id;
        private string name;
        private string size;

        public ProductEditViewModel(Product product, IDataService dataService, IEventAggregator eventAggregator)
        {
            this.dataService = dataService;
            this.eventAggregator = eventAggregator;

            id = product.Id;
            name = product.Name;
            size = product.Size;

            Title = string.Format("{0} {1}", product.Name, product.Size);
            SaveCommand = new DelegateCommand<ChildWindow>(Save);
        }

        public ICommand SaveCommand { get; private set; }

        public string Id { get { return id; } }

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

        private async void Save(ChildWindow window)
        {
            ValidateName();
            ValidateSize();

            if (HasErrors) return;

            var product = new Product
            {
                Id = id,
                Name = name,
                Size = size,
            };

            var task = await dataService.SaveProductAsync(product);
            if (task.Succeed)
            {
                eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(new ProductUpdatedEventArgs(id));
                Confirmed = true;
                window.Close();
            }
        }
    }
}
