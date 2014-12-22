using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.UsersModule
{
    public class UsersModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, UsersView>(Consts.UsersView);
        }
    }
}
