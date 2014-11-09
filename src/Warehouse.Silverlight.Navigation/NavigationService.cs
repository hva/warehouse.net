using Microsoft.Practices.Prism.Regions;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IRegionManager regionManager;

        public NavigationService(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
        }

        public void OpenLoginPage()
        {
            regionManager.RequestNavigate(Consts.RootRegion, Consts.LoginView);
        }

        public void OpenLandingPage()
        {
            regionManager.RequestNavigate(Consts.RootRegion, Consts.LoggedInView);
        }
    }
}
