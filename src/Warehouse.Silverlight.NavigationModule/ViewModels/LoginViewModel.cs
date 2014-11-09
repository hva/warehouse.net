using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Warehouse.Silverlight.NavigationModule.ViewModels
{
    public class LoginViewModel
    {
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(DoLogin);
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; set; }

        private void DoLogin()
        {
            
        }
    }
}
