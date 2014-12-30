using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Infrastructure;

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
