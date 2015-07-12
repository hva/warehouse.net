using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Wpf.Auth;

namespace Warehouse.Wpf.Module.Shell.LoggedIn
{
    public class LoggedInViewModel
    {
        private Action logoutCallback;

        public LoggedInViewModel()
        {
            NavigateToPageCommand = new DelegateCommand<string>(NavigateToPage);
            LogoutCommand = new DelegateCommand(Logout);
        }

        public LoggedInViewModel Init(AuthToken token, Action logout)
        {
            logoutCallback = logout;
            IsAdmin = token.IsAdmin();
            return this;
        }

        public bool IsAdmin { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand NavigateToPageCommand { get; private set; }

        private void NavigateToPage(string page)
        {
            //regionManager.RequestNavigate(Consts.MainRegion, page);
        }

        private void Logout()
        {
            logoutCallback();
        }
    }
}
