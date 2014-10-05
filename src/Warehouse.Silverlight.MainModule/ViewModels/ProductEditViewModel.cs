using System.ComponentModel.DataAnnotations;
using Warehouse.Silverlight.MainModule.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestValidationObject
    {
        private readonly Product product;

        public ProductEditViewModel(Product product)
        {
            this.product = product;
            Title = string.Format("{0} {1}", product.Name, product.Size);
        }

        [Required]
        public string Name
        {
            get { return product.Name; }
            set
            {
                if (product.Name != value)
                {
                    product.Name = value;
                    ValidateName();
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private void ValidateName()
        {
            errorsContainer.ClearErrors(() => Name);
            errorsContainer.SetErrors(() => Name, Validate.Required(Name));
        }
    }
}
