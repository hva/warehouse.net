using Microsoft.Practices.Prism.Regions;

namespace Warehouse.Wpf.Module.ProductDetail.ViewModels
{
    public class CreateProductViewModel : INavigationAware
    {
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
