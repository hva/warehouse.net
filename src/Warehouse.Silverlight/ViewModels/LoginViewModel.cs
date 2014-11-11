using System.IO.IsolatedStorage;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.DataService.Auth;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.ViewModels
{
    public class LoginViewModel : NotificationObject, INavigationAware
    {
        private int messageOpacity;
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        private string login;
        private string password;

        public LoginViewModel(IAuthService authService, INavigationService navigationService)
        {
            this.authService = authService;
            this.navigationService = navigationService;

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

        public int MessageOpacity
        {
            get { return messageOpacity; }
            set { messageOpacity = value; RaisePropertyChanged(() => MessageOpacity); }
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
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            IsolatedStorageSettings.ApplicationSettings[Consts.SettingsLoginKey] = Login;
        }

        #endregion

        private async void DoLogin()
        {
            var task = await authService.Login(Login, Password);
            if (task.Succeed)
            {
                MessageOpacity = 0;
                navigationService.OpenLandingPage();
            }
            else
            {
                MessageOpacity = 1;
            }
        }
    }
}
