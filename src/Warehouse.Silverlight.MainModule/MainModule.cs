using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.MainModule.Views;

namespace Warehouse.Silverlight.MainModule
{
    public class MainModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, MainView>(Consts.MainView);
        }
    }
}
