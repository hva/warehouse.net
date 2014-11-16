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
            // navigating MainRegion to fake view to invoke OnNavigatedFrom on current view
            // TODO: think about another way
            regionManager.RequestNavigate(Consts.MainRegion, string.Empty);

            regionManager.RequestNavigate(Consts.RootRegion, Consts.LoginView);
        }

        public void OpenLandingPage()
        {
            regionManager.RequestNavigate(Consts.RootRegion, Consts.LoggedInView);

            regionManager.RequestNavigate(Consts.TopMenuRegion, Consts.TopMenu);
            regionManager.RequestNavigate(Consts.MainRegion, Consts.MainView);
        }
    }
}
