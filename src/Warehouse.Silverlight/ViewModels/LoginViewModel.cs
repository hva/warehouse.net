using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.DataService;

namespace Warehouse.Silverlight.ViewModels
{
    public class LoginViewModel
    {
        private readonly IAuthService authService;

        public LoginViewModel(IAuthService authService)
        {
            this.authService = authService;

            LoginCommand = new DelegateCommand(DoLogin);
        }

        public string Login { get; set; }
        public string Password { get; set; }
        public ICommand LoginCommand { get; set; }

        private async void DoLogin()
        {
            var task = await authService.Login(Login, Password);
            if (task.Success)
            {
                
            }
        }
    }
}
