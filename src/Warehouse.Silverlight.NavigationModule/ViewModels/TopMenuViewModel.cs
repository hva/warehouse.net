using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.DataService.Auth;
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.NavigationModule.ViewModels
{
    public class TopMenuViewModel
    {
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;

        public TopMenuViewModel(IAuthService authService, INavigationService navigationService)
        {
            this.authService = authService;
            this.navigationService = navigationService;

            LogoutContent = string.Format("Выйти ({0})", authService.Token.UserName);
            LogoutCommand = new DelegateCommand(Logout);
        }

        public string LogoutContent { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private void Logout()
        {
            authService.Logout();
            navigationService.OpenLoginPage();
        }
    }
}
