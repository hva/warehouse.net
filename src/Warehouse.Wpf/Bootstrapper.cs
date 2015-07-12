using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Data;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Module.Main;
using Warehouse.Wpf.Module.ProductDetail.Create;
using Warehouse.Wpf.Module.ProductDetail.Edit;
using Warehouse.Wpf.Module.Shell;
using Warehouse.Wpf.SignalR;
using Warehouse.Wpf.SignalR.Interfaces;

namespace Warehouse.Wpf
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IAuthStore, AuthStore>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IAuthService, AuthService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IApplicationSettings, ApplicationSettings>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ISignalRClient, SignalRClient>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IProductsRepository, ProductsRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IFilesRepository, FilesRepository>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(ResolvViewTypeFromViewModelType);

            PageLocator.SetOpenWindowCallback(OpenWindow);
            PageLocator.Register<MainView>(PageName.ProductsList);
            PageLocator.Register<ProductCreateWindow>(PageName.ProductCreateWindow);
            PageLocator.Register<ProductEditWindow>(PageName.ProductEditWindow);
        }

        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
            ViewModelLocator.SetAutoWireViewModel(Shell, true);
        }

        private Type ResolvViewTypeFromViewModelType(Type viewType)
        {
            var viewName = viewType.FullName;
            viewName = viewName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
            viewModelName = viewModelName.Replace("ViewViewModel", "ViewModel");
            return Type.GetType(viewModelName);
        }

        private void OpenWindow(object page, object param)
        {
            var window = page as Window;
            if (window != null)
            {
                window.Owner = (Window) Shell;

                var vm = window.DataContext as INavigationAware;
                if (vm != null)
                {
                    vm.OnNavigatedTo(param);
                }

                window.ShowDialog();
            }
        }
    }
}
