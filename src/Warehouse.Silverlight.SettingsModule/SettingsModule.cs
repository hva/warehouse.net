using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.SettingsModule.Views;

namespace Warehouse.Silverlight.SettingsModule
{
    public class SettingsModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, SettingsView>(Consts.SettingsView);
        }
    }
}
