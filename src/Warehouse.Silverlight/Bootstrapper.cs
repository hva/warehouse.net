using System.Net;
using System.Net.Browser;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.DataService;
using Warehouse.Silverlight.DataService.Auth;
using Warehouse.Silverlight.DataService.Log;
using Warehouse.Silverlight.NavigationModule;
using Warehouse.Silverlight.SignalRModule;

namespace Warehouse.Silverlight
{
    public class Bootstrapper : UnityBootstrapper
    {
        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);

            ((FrameworkElement)Shell).Language = XmlLanguage.GetLanguage(Thread.CurrentThread.CurrentCulture.Name);
            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);

            var authService = Container.Resolve<IAuthService>();
            var navigationService = Container.Resolve<INavigationService>();
            if (authService.IsValid())
            {
                navigationService.OpenLandingPage();
            }
            else
            {
                navigationService.OpenLoginPage();
            }

            var signalR = Container.Resolve<ISignalRClient>();
            signalR.Start();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(MainModule.MainModule));
            moduleCatalog.AddModule(typeof(SignalRModule.SignalRModule));
            moduleCatalog.AddModule(typeof(NavigationModule.NavigationModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDataService, DataService.DataService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILogger, BrowserLogger>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();
            Application.Current.RootVisual = (UIElement)Shell;
        }
    }
}
