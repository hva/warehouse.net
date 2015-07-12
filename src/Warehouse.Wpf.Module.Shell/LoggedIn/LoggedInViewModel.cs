using System;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Infrastructure.Interfaces;

namespace Warehouse.Wpf.Module.Shell.LoggedIn
{
    public class LoggedInViewModel : BindableBase
    {
        private Action logoutCallback;
        private IView view;

        public LoggedInViewModel()
        {
            NavigateToPageCommand = new DelegateCommand<string>(NavigateToPage);
            LogoutCommand = new DelegateCommand(Logout);
        }

        public LoggedInViewModel Init(AuthToken token, Action logout)
        {
            logoutCallback = logout;
            IsAdmin = token.IsAdmin();
            NavigateToPage(PageName.ProductsList);
            return this;
        }

        public bool IsAdmin { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand NavigateToPageCommand { get; private set; }

        public IView View
        {
            get { return view; }
            set { SetProperty(ref view, value); }
        }

        private void NavigateToPage(string page)
        {
            NavigateFromCurrentView();
            View = PageLocator.Resolve(page);
            NavigateToCurrentView();
        }

        private void Logout()
        {
            NavigateFromCurrentView();
            logoutCallback();
        }

        private void NavigateFromCurrentView()
        {
            var vm = GetViewModel();
            if (vm != null)
            {
                vm.OnNavigatedFrom();
            }
        }

        private void NavigateToCurrentView()
        {
            var vm = GetViewModel();
            if (vm != null)
            {
                vm.OnNavigatedTo();
            }
        }

        private INavigationAware GetViewModel()
        {
            if (view != null)
            {
                return view.DataContext as INavigationAware;
            }
            return null;
        }
    }
}
