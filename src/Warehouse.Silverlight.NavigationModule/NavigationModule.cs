using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.NavigationModule.Views;

namespace Warehouse.Silverlight.NavigationModule
{
    public class NavigationModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, LoginView>(Consts.LoginView);
            Container.RegisterType<object, LoggedInView>(Consts.LoggedInView);
            Container.RegisterType<object, TopMenu>(Consts.TopMenu);
        }
    }
}
