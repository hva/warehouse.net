using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Data;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Module.Main;
using Warehouse.Wpf.Module.ProductDetail;
using Warehouse.Wpf.Module.Shell;
using Warehouse.Wpf.Settings;
using Warehouse.Wpf.SignalR;

namespace Warehouse.Wpf
{
    public class Bootstrapper : UnityBootstrapper
    {
        public override void Run(bool runWithDefaultConfiguration)
        {
            base.Run(runWithDefaultConfiguration);

            //var authStore = Container.Resolve<IAuthStore>();
            //var navigationService = Container.Resolve<INavigationService>();
            //var token = authStore.LoadToken();
            //if (token != null && token.IsAuthenticated())
            //{
            //    navigationService.OpenLandingPage();
            //}
            //else
            //{
            //    navigationService.OpenLoginPage();
            //}
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(ShellModule));
            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(MainModule));
            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(ProductDetailModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<Window, Shell>(new ContainerControlledLifetimeManager());
            //Container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthStore, AuthStore>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IApplicationSettings, ApplicationSettings>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProductsRepository, ProductsRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFilesRepository, FilesRepository>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }
    }
}
