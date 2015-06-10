using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.Module.Main
{
    public class MainModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            Container.RegisterType<object, MainView>(Consts.MainView);

            ViewModelLocationProvider.Register(typeof(MainView).FullName, () => Container.Resolve<MainViewModel>());
        }
    }
}
