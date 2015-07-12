using System;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Module.Shell.LoggedIn;
using Warehouse.Wpf.Module.Shell.Login;
using Warehouse.Wpf.SignalR.Interfaces;

namespace Warehouse.Wpf.Module.Shell
{
    public class ShellViewModel : BindableBase
    {
        private object context;

        private readonly IAuthStore authStore;
        private readonly IAuthService authService;
        private readonly ISignalRClient signalRClient;
        private readonly Func<LoginViewModel> loginFactory;
        private readonly Func<LoggedInViewModel> loggedInFactory;

        public ShellViewModel(IAuthStore authStore, IAuthService authService, ISignalRClient signalRClient,
            Func<LoginViewModel> loginFactory, Func<LoggedInViewModel> loggedInFactory)
        {
            this.authStore = authStore;
            this.authService = authService;
            this.signalRClient = signalRClient;
            this.loginFactory = loginFactory;
            this.loggedInFactory = loggedInFactory;

            Refresh();
        }

        public object Context
        {
            get { return context; }
            set { SetProperty(ref context, value); }
        }

        private void Refresh()
        {
            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                Context = loggedInFactory().Init(token, DoLogout);
            }
            else
            {
                Context = loginFactory().Init(DoLogin);
            }
        }

        private async Task<bool> DoLogin(string login, string password)
        {
            var task = await authService.Login(login, password);
            if (task.Succeed)
            {
                await signalRClient.StartAsync();
                Refresh();
            }
            return task.Succeed;
        }

        private void DoLogout()
        {
            authStore.ClearToken();
            signalRClient.Stop();
            Refresh();
        }
    }
}
