using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
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
            RegionManager.RegisterViewWithRegion("NavigationRegion", () => Container.Resolve<TopMenu>());
        }
    }
}
