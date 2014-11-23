using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Data.Auth;
using Warehouse.Silverlight.Data.Users;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.SettingsModule.ViewModels
{
    public class SettingsViewModel : ValidationObject
    {
        private readonly IUsersRepository usersRepository;

        public SettingsViewModel(IAuthService authService, IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;

            if (authService.IsValid())
            {
                UserName = authService.Token.UserName;
            }

            SaveCommand = new DelegateCommand(Save);
        }

        public string UserName { get; private set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }

        public ICommand SaveCommand { get; private set; }

        private async void Save()
        {
            ValidatePassword();
            if (!HasErrors)
            {
                var task = await usersRepository.ChangePasswordAsync(UserName, OldPassword, NewPassword);
                if (task.Succeed)
                {
                    OldPassword = null;
                    NewPassword = null;
                    NewPassword2 = null;
                    MessageBox.Show("Пароль обновлен!");
                }
                else
                {
                    
                }
            }
        }

        private void ValidatePassword()
        {
            errorsContainer.ClearErrors(() => NewPassword);
            errorsContainer.SetErrors(() => NewPassword, Validate.Password(NewPassword, NewPassword2));
        }
    }
}
