using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Mvvm;
using Warehouse.Wpf.SignalR;

namespace Warehouse.Wpf.Module.Shell.LoggedIn
{
    public class TopMenuViewModel : BindableBase
    {
        private readonly IAuthStore authStore;
        private readonly INavigationService navigationService;
        private readonly ISignalRClient signalRClient;
        private bool isAdmin;

        public TopMenuViewModel(IAuthStore authStore, INavigationService navigationService, ISignalRClient signalRClient)
        {
            this.authStore = authStore;
            this.navigationService = navigationService;
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
            //regionManager.RequestNavigate(Consts.MainRegion, page);
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set { SetProperty(ref isAdmin, value); }
        }

        #region INavigationAware

        //public void OnNavigatedTo(NavigationContext navigationContext)
        //{
        //    var token = authStore.LoadToken();
        //    IsAdmin = token != null && token.IsAdmin();
        //}

        //public bool IsNavigationTarget(NavigationContext navigationContext)
        //{
        //    return true;
        //}

        //public void OnNavigatedFrom(NavigationContext navigationContext)
        //{
        //}

        #endregion
    }
}
