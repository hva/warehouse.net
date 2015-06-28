using System.Windows.Controls;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.ServiceLocation;

namespace Warehouse.Wpf.Module.Main
{
    public class RegionAwareControl : ContentControl
    {
        public RegionAwareControl()
        {
            RegionManager.SetRegionManager(this, ServiceLocator.Current.GetInstance<IRegionManager>());
        }
    }
}
