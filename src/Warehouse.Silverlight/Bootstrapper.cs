using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.DataService;

namespace Warehouse.Silverlight
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            ((FrameworkElement)Shell).Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            Application.Current.RootVisual = (UIElement)Shell;
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
            //moduleCatalog.AddModule(typeof(MainModule.MainModule));
            moduleCatalog.AddModule(typeof(NavigationModule.NavigationModule));
            //moduleCatalog.AddModule(typeof(SignalRModule.SignalRModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IDataService, DataService.DataService>(new ContainerControlledLifetimeManager());
        }
    }
}
