using System;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Module.Shell.LoggedIn;
using Warehouse.Wpf.Module.Shell.Login;

namespace Warehouse.Wpf.Module.Shell
{
    public class ShellViewModel : BindableBase
    {
        private INavigationAware context;

        private readonly IAuthStore authStore;
        private readonly Func<LoginViewModel> loginFactory;
        private readonly Func<LoggedInViewModel> loggedInFactory;

        public ShellViewModel(IAuthStore authStore, Func<LoginViewModel> loginFactory, Func<LoggedInViewModel> loggedInFactory)
        {
            this.authStore = authStore;
            this.loginFactory = loginFactory;
            this.loggedInFactory = loggedInFactory;

            Refresh();
        }

        public INavigationAware Context
        {
            get { return context; }
            set { SetProperty(ref context, value); }
        }

        private void Refresh()
        {
            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                Context = loggedInFactory();
            }
            else
            {
                if (context != null)
                {
                    context.OnNavigatedFrom();
                }
                Context = loginFactory();
            }
        }
    }
}
