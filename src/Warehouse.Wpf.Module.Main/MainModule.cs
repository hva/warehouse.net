using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;

namespace Warehouse.Wpf.Module.Main
{
    public class MainModule : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        public void Initialize()
        {
            ViewModelLocationProvider.Register(typeof(MainView).FullName, () => Container.Resolve<MainViewModel>());
        }
    }
}
