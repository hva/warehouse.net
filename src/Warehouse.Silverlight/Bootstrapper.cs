using System.Net;
using System.Net.Browser;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Log;
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

            var authStore = Container.Resolve<IAuthStore>();
            var navigationService = Container.Resolve<INavigationService>();
            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
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

            var moduleCatalog = (ModuleCatalog)ModuleCatalog;
            moduleCatalog.AddModule(typeof(MainModule.MainModule));
            moduleCatalog.AddModule(typeof(SettingsModule.SettingsModule));
            moduleCatalog.AddModule(typeof(NavigationModule.NavigationModule));
            moduleCatalog.AddModule(typeof(UsersModule.UsersModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IUsersRepository, UsersRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProductsRepository, ProductsRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFilesRepository, FilesRepository>(new ContainerControlledLifetimeManager());

            Container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthStore, AuthStore>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ILogger, BrowserLogger>(new ContainerControlledLifetimeManager());

            Container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());
            
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
