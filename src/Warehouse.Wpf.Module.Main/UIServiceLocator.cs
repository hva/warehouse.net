using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.Module.Main
{
    public static class UIServiceLocator
    {
        public static IRegionManager RegionManager { get { return ServiceLocator.Current.GetInstance<IRegionManager>(); } }
    }
}
