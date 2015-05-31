using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.Module.Shell
{
    public class ShellModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, LoginView>(Consts.LoginView);
            //Container.RegisterType<object, LoggedInView>(Consts.LoggedInView);
            //Container.RegisterType<object, TopMenu>(Consts.TopMenu);
        }
    }
}
