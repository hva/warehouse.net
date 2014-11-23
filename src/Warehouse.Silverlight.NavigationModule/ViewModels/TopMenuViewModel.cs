using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Warehouse.Silverlight.Data.Auth;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.NavigationModule.ViewModels
{
    public class TopMenuViewModel
    {
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        private readonly IRegionManager regionManager;

        public TopMenuViewModel(IAuthService authService, INavigationService navigationService, IRegionManager regionManager)
        {
            this.authService = authService;
            this.navigationService = navigationService;
            this.regionManager = regionManager;

            LogoutContent = string.Format("Выйти ({0})", authService.Token.UserName);
            LogoutCommand = new DelegateCommand(Logout);

            NavigateToPageCommand = new DelegateCommand<string>(NavigateToPage);
        }

        public string LogoutContent { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        public ICommand NavigateToPageCommand { get; private set; }

        private void Logout()
        {
            authService.Logout();
            navigationService.OpenLoginPage();
        }

        private void NavigateToPage(string page)
        {
            regionManager.RequestNavigate(Consts.MainRegion, page);
        }
    }
}
