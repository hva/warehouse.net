using System;
using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Module.Users.Converters;
using Warehouse.Wpf.Mvvm;

namespace Warehouse.Wpf.Module.Users
{
    public class CreateUserViewModel : ValidationObject, IConfirmation, IInteractionRequestAware
    {
        private readonly IUsersRepository repository;
        private bool isBusy;
        private string name;
        private string password;
        private string error;

        public CreateUserViewModel(IUsersRepository repository)
        {
            this.repository = repository;

            Title = "Новый пользователь";

            Roles = new Dictionary<string, string>
            {
                { UserRole.Editor, RoleToStringConverter.RoleTranslations[UserRole.Editor] },
                { UserRole.User, RoleToStringConverter.RoleTranslations[UserRole.User] },
            };
            Role = UserRole.User;
            SaveCommand = new DelegateCommand(Save);
            CloseCommand = new DelegateCommand(Close);
        }

        #region IConfirmation, IInteractionRequestAware
        public string Title { get; set; }
        public object Content { get; set; }
        public bool Confirmed { get; set; }
        public INotification Notification { get; set; }
        public Action FinishInteraction { get; set; }
        #endregion

        public ICommand SaveCommand { get; private set; }
        public ICommand CloseCommand { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        #region Name

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    ValidateName();
                }
            }
        }

        private void ValidateName()
        {
            errorsContainer.ClearErrors(() => Name);
            errorsContainer.SetErrors(() => Name, Validate.Required(Name));
        }

        #endregion

        #region Role

        public IDictionary<string, string> Roles { get; private set; }
        public string Role { get; set; }

        #endregion

        #region Password

        public string Password
        {
            get { return password; }
            set
            {
                if (password != value)
                {
                    password = value;
                    ValidatePassword();
                }
            }
        }

        private void ValidatePassword()
        {
            errorsContainer.ClearErrors(() => Password);
            errorsContainer.SetErrors(() => Password, Validate.Required(Password));
        }

        #endregion

        #region Error

        public string Error
        {
            get { return error; }
            set { SetProperty(ref error, value); }
        }

        #endregion

        private async void Save()
        {
            Error = null;

            ValidateName();
            ValidatePassword();
            if (HasErrors) return;

            var user = new User
            {
                UserName = Name,
                Roles = new [] { Role },
                Password = Password,
            };

            IsBusy = true;
            var task = await repository.CreateUser(user);
            IsBusy = false;
            if (task.Succeed)
            {
                Confirmed = true;
                Close();
            }
            else
            {
                Error = task.ErrorMessage;
            }
        }

        private void Close()
        {
            if (FinishInteraction != null)
            {
                FinishInteraction();
            }
        }
    }
}
