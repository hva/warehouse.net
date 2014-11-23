using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Data.Auth;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.SettingsModule.ViewModels
{
    public class SettingsViewModel : ValidationObject
    {
        public SettingsViewModel(IAuthService authService)
        {
            if (authService.IsValid())
            {
                UserName = authService.Token.UserName;
            }

            SaveCommand = new DelegateCommand(Save);
        }

        public string UserName { get; private set; }
        public string Password { get; set; }
        public string Password2 { get; set; }

        public ICommand SaveCommand { get; private set; }

        private void Save()
        {
            ValidatePassword();
            if (!HasErrors)
            {
                
            }
        }

        private void ValidatePassword()
        {
            errorsContainer.ClearErrors(() => Password);
            errorsContainer.SetErrors(() => Password, Validate.Password(Password, Password2));
        }
    }
}
