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
    public class TopMenuViewModel : NotificationObject, INavigationAware
    {
        private readonly IAuthStore authStore;
        private readonly INavigationService navigationService;
        private readonly IRegionManager regionManager;
        private readonly ISignalRClient signalRClient;
        private bool isAdmin;

        public TopMenuViewModel(IAuthStore authStore, INavigationService navigationService,
            IRegionManager regionManager, ISignalRClient signalRClient)
        {
            this.authStore = authStore;
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
            authStore.ClearToken();
            signalRClient.Stop();
            navigationService.OpenLoginPage();
        }

        private void NavigateToPage(string page)
        {
            regionManager.RequestNavigate(Consts.MainRegion, page);
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                if (isAdmin != value)
                {
                    isAdmin = value;
                    RaisePropertyChanged(() => IsAdmin);
                }
            }
        }

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var token = authStore.LoadToken();
            IsAdmin = token != null && token.IsAdmin();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion
    }
}
