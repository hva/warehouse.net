using System.Net;
using System.Net.Browser;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Data;
using Warehouse.Silverlight.Data.Auth;
using Warehouse.Silverlight.Data.Log;
using Warehouse.Silverlight.Data.Users;
using Warehouse.Silverlight.Navigation;
using Warehouse.Silverlight.SignalR;

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
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ModuleCatalog moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(MainModule.MainModule));
            moduleCatalog.AddModule(typeof(SettingsModule.SettingsModule));
            moduleCatalog.AddModule(typeof(NavigationModule.NavigationModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ILogger, BrowserLogger>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IUsersRepository, UsersRepository>(new ContainerControlledLifetimeManager());
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
