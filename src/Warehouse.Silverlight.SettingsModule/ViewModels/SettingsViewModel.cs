using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Regions;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Users;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.SettingsModule.ViewModels
{
    public class SettingsViewModel : ValidationObject, INavigationAware
    {
        private readonly IAuthStore authStore;
        private readonly IUsersRepository usersRepository;
        private string userName;
        private string role;
        private string errorMessage;
        private string successMessage;

        public SettingsViewModel(IAuthStore authStore, IUsersRepository usersRepository)
        {
            this.authStore = authStore;
            this.usersRepository = usersRepository;

            SaveCommand = new DelegateCommand(Save);
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(() => UserName); }
        }

        public string Role
        {
            get { return role; }
            set { role = value; RaisePropertyChanged(() => Role); }
        }

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
                    RaisePropertyChanged(() => ErrorMessageOpacity);
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
                    RaisePropertyChanged(() => SuccessMessageOpacity);
                }
            }
        }

        public double ErrorMessageOpacity { get { return string.IsNullOrEmpty(errorMessage) ? 0 : 1; } }
        public double SuccessMessageOpacity { get { return string.IsNullOrEmpty(successMessage) ? 0 : 1; } }

        public ICommand SaveCommand { get; private set; }

        private async void Save()
        {
            ErrorMessage = null;
            SuccessMessage = null;

            ValidateOldPassword();
            ValidateNewPassword();

            if (!HasErrors)
            {
                var task = await usersRepository.ChangePasswordAsync(UserName, OldPassword, NewPassword);
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

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                UserName = token.UserName;
                Role = token.Role;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            UserName = null;
            Role = null;
        }

        #endregion
    }
}
