using System;
using System.Reflection;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Auth;
using Warehouse.Wpf.Auth.Interfaces;

namespace Warehouse.Wpf.Module.Settings
{
    public class SettingsViewModel : BindableBase
    {
        private readonly InteractionRequest<IConfirmation> changePasswordRequest;
        private readonly Func<ChangePasswordViewModel> changePasswordFactory;
        private string successMessage;

        public SettingsViewModel(IAuthStore authStore, Func<ChangePasswordViewModel> changePasswordFactory)
        {
            this.changePasswordFactory = changePasswordFactory;

            var v = Assembly.GetEntryAssembly().GetName().Version;
            Version = string.Join(".", new[] { v.Major, v.Minor, v.Build });

            changePasswordRequest = new InteractionRequest<IConfirmation>();
            ChangePasswordCommand = new DelegateCommand(ChangePassword);

            var token = authStore.LoadToken();
            if (token != null && token.IsAuthenticated())
            {
                UserName = token.UserName;
                Role = token.Role;
            }
        }

        private void ChangePassword()
        {
            SuccessMessage = null;

            var vm = changePasswordFactory().Init(UserName);

            changePasswordRequest.Raise(vm, x =>
            {
                if (x.Confirmed)
                {
                    SuccessMessage = "Пароль обновлен!";
                }
            });
        }

        public string Version { get; private set; }
        public ICommand ChangePasswordCommand { get; private set; }
        public IInteractionRequest ChangePasswordRequest { get { return changePasswordRequest; } }

        public string UserName { get; private set; }
        public string Role { get; private set; }

        public string SuccessMessage
        {
            get { return successMessage; }
            set { SetProperty(ref successMessage, value); }
        }
    }
}
