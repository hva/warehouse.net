using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;
using Warehouse.Wpf.SignalR;
using Warehouse.Wpf.SignalR.Interfaces;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Data;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Module.ChangePrice;
using Warehouse.Wpf.Module.Main;
using Warehouse.Wpf.Module.ProductDetail.Create;
using Warehouse.Wpf.Module.ProductDetail.Edit;
using Warehouse.Wpf.UI.Modules.Files;
using Warehouse.Wpf.UI.Modules.Shell;
using Warehouse.Wpf.UI.Modules.Transactions;
using Warehouse.Wpf.UI.Modules.Settings;
using Warehouse.Wpf.UI.Modules.Users;

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
            Container.RegisterType<IUsersRepository, UsersRepository>(new ContainerControlledLifetimeManager());
            Container.RegisterType<ITransactionsRepository, TransactionsRepository>(new ContainerControlledLifetimeManager());

            ViewModelLocationProvider.SetDefaultViewModelFactory(x => Container.Resolve(x));
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(ResolveViewTypeFromViewModelType);

            PageLocator.Register<MainView>(PageName.ProductsList);
            PageLocator.Register<FilesView>(PageName.FilesList);
            PageLocator.Register<ProductCreateWindow>(PageName.ProductCreateWindow);
            PageLocator.Register<ProductEditWindow>(PageName.ProductEditWindow);
            PageLocator.Register<ChangePriceWindow>(PageName.ChangePriceWindow);
            PageLocator.Register<TransactionsView>(PageName.OperationsList);
            PageLocator.Register<UsersListView>(PageName.Users);
            PageLocator.Register<SettingsView>(PageName.Settings);
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

        private Type ResolveViewTypeFromViewModelType(Type viewType)
        {
            var viewName = viewType.FullName;
            viewName = viewName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
            viewModelName = viewModelName.Replace("ViewViewModel", "ViewModel");
            return Type.GetType(viewModelName);
        }
    }
}
