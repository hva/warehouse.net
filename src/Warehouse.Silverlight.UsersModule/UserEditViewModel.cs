using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Warehouse.Silverlight.Data.Users;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.UsersModule
{
    public class UserEditViewModel : InteractionRequestValidationObject
    {
        private readonly IUsersRepository repository;
        private string name;
        private string password;
        private string error;

        public UserEditViewModel(IUsersRepository repository)
        {
            this.repository = repository;

            Roles = new Dictionary<string, string>
            {
                {"editor", "Менеджер"},
                {"user", "Кладовщик"},
            };
            Role = "user";
            SaveCommand = new DelegateCommand<ChildWindow>(Save);
        }

        public ICommand SaveCommand { get; private set; }

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
                    RaisePropertyChanged(() => ErrorVisibility);
                }
            }
        }

        public Visibility ErrorVisibility
        {
            get { return string.IsNullOrEmpty(error) ? Visibility.Collapsed : Visibility.Visible; }
        }

        #endregion

        private async void Save(ChildWindow window)
        {
            ValidateName();
            ValidatePassword();
            if (HasErrors) return;

            var user = new User
            {
                UserName = Name,
                Roles = new [] { Role },
                Password = Password,
            };

            var task = await repository.CreateUser(user);
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
