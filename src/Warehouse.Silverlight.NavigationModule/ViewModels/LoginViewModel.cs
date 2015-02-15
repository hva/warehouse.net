using System.IO.IsolatedStorage;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Navigation;
using Warehouse.Silverlight.SignalR;

namespace Warehouse.Silverlight.NavigationModule.ViewModels
{
    public class LoginViewModel : NotificationObject, INavigationAware, IRegionMemberLifetime
    {
        private string message;

        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        private readonly ISignalRClient signalRClient;

        private string login;
        private string password;
        private bool isBusy;

        public LoginViewModel(IAuthService authService, INavigationService navigationService, ISignalRClient signalRClient)
        {
            this.authService = authService;
            this.navigationService = navigationService;
            this.signalRClient = signalRClient;

            LoginCommand = new DelegateCommand(DoLogin);
        }

        public string Login
        {
            get { return login; }
            set { login = value; RaisePropertyChanged(() => Login); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; RaisePropertyChanged(() => Password); }
        }

        public ICommand LoginCommand { get; set; }

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(() => Message); }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    RaisePropertyChanged(() => IsBusy);
                }
            }
        }

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            string l;
            if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(Consts.SettingsLoginKey, out l))
            {
                Login = l;
            }
            Password = string.Empty;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            IsolatedStorageSettings.ApplicationSettings[Consts.SettingsLoginKey] = Login;
        }

        #endregion

        #region IRegionMemberLifetime

        public bool KeepAlive { get { return false; } }

        #endregion

        private async void DoLogin()
        {
            IsBusy = true;
            Message = null;
            var task = await authService.Login(Login, Password);

            if (task.Succeed)
            {
                await signalRClient.StartAsync();
                navigationService.OpenLandingPage();
            }
            else
            {
                Password = string.Empty;
                Message = "Пароль, который вы ввели, неверный.\nПожалуйста, попробуйте еще раз.";
            }
            IsBusy = false;
        }
    }
}
