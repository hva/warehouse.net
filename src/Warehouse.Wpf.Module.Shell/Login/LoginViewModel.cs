using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Infrastructure.Interfaces;
using Warehouse.Wpf.Mvvm;
using Warehouse.Wpf.SignalR;

namespace Warehouse.Wpf.Module.Shell.Login
{
    public class LoginViewModel : BindableBase, INavigationAware
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

        public void OnNavigatedTo(object param)
        {
            //string l;
            //if (IsolatedStorageSettings.ApplicationSettings.TryGetValue(Consts.SettingsLoginKey, out l))
            //{
            //    Login = l;
            //}
            //Password = string.Empty;
        }

        public void OnNavigatedFrom()
        {
            //IsolatedStorageSettings.ApplicationSettings[Consts.SettingsLoginKey] = Login;
        }

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
