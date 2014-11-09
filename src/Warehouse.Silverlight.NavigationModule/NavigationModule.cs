using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.NavigationModule.Views;

namespace Warehouse.Silverlight.NavigationModule
{
    public class NavigationModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IRegionManager RegionManager { get; set; }

        public void Initialize()
        {
            // registering view types
            Container.RegisterType<object, LoginView>(Consts.LoginView);

            // init view
            RegionManager.RequestNavigate(Consts.RootRegion, new Uri(Consts.LoginView, UriKind.Relative));

            //RegionManager.RegisterViewWithRegion("NavigationRegion", () => Container.Resolve<TopMenu>());
            //RegionManager.RequestNavigate("MainRegion", new Uri("MainView", UriKind.Relative));
        }
    }
}
