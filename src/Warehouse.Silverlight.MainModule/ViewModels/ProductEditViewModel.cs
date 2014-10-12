using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.MainModule.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestValidationObject
    {
        private readonly IDataService dataService;
        private readonly string id;
        private string name;

        public ProductEditViewModel(Product product, IDataService dataService)
        {
            this.dataService = dataService;

            id = product.Id;
            name = product.Name;

            Title = string.Format("{0} {1}", product.Name, product.Size);
            SaveCommand = new DelegateCommand<ChildWindow>(Save);
        }

        public ICommand SaveCommand { get; private set; }

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

        private async void Save(ChildWindow window)
        {
            ValidateName();

            if (HasErrors) return;

            var product = new Product
            {
                Id = id,
                Name = name,
            };

            var task = await dataService.SaveProductAsync(product);
            if (task.Success)
            {
                Confirmed = true;
                window.Close();
            }
        }

        private void ValidateName()
        {
            errorsContainer.ClearErrors(() => Name);
            errorsContainer.SetErrors(() => Name, Validate.Required(Name));
        }
    }
}
