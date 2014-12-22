using Microsoft.Practices.Prism.Regions;

namespace Warehouse.Silverlight.UsersModule
{
    public class UsersViewModel : INavigationAware
    {
        #region INavigationAware

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

        #endregion
    }
}
