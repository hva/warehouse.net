using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Controls.Converters;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.UsersModule
{
    public class CreateUserViewModel : InteractionRequestValidationObject
    {
        private readonly IUsersRepository repository;
        private bool isBusy;
        private string name;
        private string password;
        private string error;

        public CreateUserViewModel(IUsersRepository repository)
        {
            this.repository = repository;

            Roles = new Dictionary<string, string>
            {
                { UserRole.Editor, RoleToStringConverter.RoleTranslations[UserRole.Editor] },
                { UserRole.User, RoleToStringConverter.RoleTranslations[UserRole.User] },
            };
            Role = UserRole.User;
            SaveCommand = new DelegateCommand<ChildWindow>(Save);
        }

        public ICommand SaveCommand { get; private set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
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
            set
            {
                if (error != value)
                {
                    error = value;
                    RaisePropertyChanged(() => Error);
                }
            }
        }

        #endregion

        private async void Save(ChildWindow window)
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
                window.Close();
            }
            else
            {
                Error = task.ErrorMessage;
            }
        }
    }
}
