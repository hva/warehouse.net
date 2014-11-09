using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.DataService.Auth;
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.ViewModels
{
    public class LoginViewModel : NotificationObject
    {
        private int messageOpacity;
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;

        public LoginViewModel(IAuthService authService, INavigationService navigationService)
        {
            this.authService = authService;
            this.navigationService = navigationService;

            LoginCommand = new DelegateCommand(DoLogin);
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; set; }

        public int MessageOpacity
        {
            get { return messageOpacity; }
            set { messageOpacity = value; RaisePropertyChanged(() => MessageOpacity); }
        }

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
