using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Regions;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Navigation;
using Warehouse.Wpf.SignalR;

namespace Warehouse.Wpf.Module.Shell
{
    public class LoginViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private string message;

        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        private readonly ISignalRClient signalRClient;

        private string login;
        private bool isBusy;

        public LoginViewModel(IAuthService authService, INavigationService navigationService, ISignalRClient signalRClient)
        {
            this.authService = authService;
            this.navigationService = navigationService;
            this.signalRClient = signalRClient;

            LoginCommand = new DelegateCommand<PasswordBox>(DoLogin);
        }

        public ICommand LoginCommand { get; private set; }

        public string Login
        {
            get { return login; }
            set { SetProperty(ref login, value); }
        }

        public string Message
        {
            get { return message; }
            set { SetProperty(ref message, value); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            //string l;
            //if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(Consts.SettingsLoginKey, out l))
            //{
            //    Login = l;
            //}
            //Password = string.Empty;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //IsolatedStorageSettings.ApplicationSettings[Consts.SettingsLoginKey] = Login;
        }

        #endregion

        #region IRegionMemberLifetime

        public bool KeepAlive { get { return false; } }

        #endregion

        private async void DoLogin(PasswordBox password)
        {
            IsBusy = true;
            Message = null;
            var task = await authService.Login(Login, password.Password);

            if (task.Succeed)
            {
                await signalRClient.StartAsync();
                navigationService.OpenLandingPage();
            }
            else
            {
                password.Password = string.Empty;
                Message = "Пароль, который вы ввели, неверный.\nПожалуйста, попробуйте еще раз.";
            }
            IsBusy = false;
        }
    }
}
