using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Warehouse.Silverlight.Data.Auth;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Navigation;
using Warehouse.Silverlight.SignalR;

namespace Warehouse.Silverlight.NavigationModule.ViewModels
{
    public class TopMenuViewModel
    {
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;
        private readonly IRegionManager regionManager;
        private readonly ISignalRClient signalRClient;

        public TopMenuViewModel(IAuthService authService, INavigationService navigationService,
            IRegionManager regionManager, ISignalRClient signalRClient)
        {
            this.authService = authService;
            this.navigationService = navigationService;
            this.regionManager = regionManager;
            this.signalRClient = signalRClient;

            LogoutCommand = new DelegateCommand(Logout);

            NavigateToPageCommand = new DelegateCommand<string>(NavigateToPage);
        }

        public ICommand LogoutCommand { get; private set; }

        public ICommand NavigateToPageCommand { get; private set; }

        private void Logout()
        {
            authService.Logout();
            signalRClient.Stop();
            navigationService.OpenLoginPage();
        }

        private void NavigateToPage(string page)
        {
            regionManager.RequestNavigate(Consts.MainRegion, page);
        }
    }
}
