using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Data;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Module.Main;
using Warehouse.Wpf.Module.ProductDetail;
using Warehouse.Wpf.Module.Shell;
using Warehouse.Wpf.Settings;
using Warehouse.Wpf.SignalR;

namespace Warehouse.Wpf
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            ((ModuleCatalog)ModuleCatalog).AddModule(typeof(ProductDetailModule));
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<Window, Shell>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthStore, AuthStore>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IApplicationSettings, ApplicationSettings>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProductsRepository, ProductsRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFilesRepository, FilesRepository>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
                viewModelName = viewModelName.Replace("ViewViewModel", "ViewModel");
                return Type.GetType(viewModelName);
            });

            PageLocator.Register<MainView>(PageName.ProductsList);
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
