using Warehouse.Silverlight.MainModule.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ProductEditViewModel : InteractionRequestViewModel
    {
        public ProductEditViewModel(Product product)
        {
            Title = string.Format("{0} {1}", product.Name, product.Size);
        }
    }
}
