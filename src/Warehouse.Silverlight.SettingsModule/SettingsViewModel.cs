using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.SettingsModule
{
    public class SettingsViewModel : ValidationObject, IRegionMemberLifetime
    {
        private readonly IUsersRepository usersRepository;
        private string errorMessage;
        private string successMessage;
        private bool isBusy;

        public SettingsViewModel(IAuthStore authStore, IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;

            SaveCommand = new DelegateCommand(Save);

            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                UserName = token.UserName;
                Role = token.Role;
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public string UserName { get; private set; }
        public string Role { get; private set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    RaisePropertyChanged(() => ErrorMessage);
                }
            }
        }

        public string SuccessMessage
        {
            get { return successMessage; }
            set
            {
                if (successMessage != value)
                {
                    successMessage = value;
                    RaisePropertyChanged(() => SuccessMessage);
                }
            }
        }

        public ICommand SaveCommand { get; private set; }

        public bool KeepAlive { get { return false; } }

        private async void Save()
        {
            ErrorMessage = null;
            SuccessMessage = null;

            ValidateOldPassword();
            ValidateNewPassword();

            if (!HasErrors)
            {
                IsBusy = true;
                var task = await usersRepository.ChangePasswordAsync(UserName, OldPassword, NewPassword);
                IsBusy = false;
                if (task.Succeed)
                {
                    SuccessMessage = "Пароль обновлен!";
                }
                else
                {
                    ErrorMessage = task.ErrorMessage;
                }
            }
        }

        private void ValidateOldPassword()
        {
            errorsContainer.ClearErrors(() => OldPassword);
            errorsContainer.SetErrors(() => OldPassword, Validate.Required(OldPassword));
        }

        private void ValidateNewPassword()
        {
            errorsContainer.ClearErrors(() => NewPassword);
            errorsContainer.SetErrors(() => NewPassword, Validate.Password(NewPassword, NewPassword2));
        }

    }
}
