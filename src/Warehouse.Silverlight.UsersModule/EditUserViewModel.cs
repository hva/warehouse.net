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
    public class EditUserViewModel : InteractionRequestValidationObject
    {
        private readonly IUsersRepository repository;
        private bool isBusy;

        public EditUserViewModel(IUsersRepository repository, User user)
        {
            this.repository = repository;

            Title = user.UserName;

            Roles = new Dictionary<string, string>
            {
                { UserRole.Editor, RoleToStringConverter.RoleTranslations[UserRole.Editor] },
                { UserRole.User, RoleToStringConverter.RoleTranslations[UserRole.User] },
            };
            Role = user.Roles[0];

            SaveCommand = new DelegateCommand<ChildWindow>(Save);
        }

        public ICommand SaveCommand { get; private set; }
        public IDictionary<string, string> Roles { get; private set; }
        public string Role { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private async void Save(ChildWindow window)
        {
            var user = new User
            {
                UserName = Title,
                Roles = new[] { Role },
            };

            IsBusy = true;
            var task = await repository.UpdateUser(user);
            IsBusy = false;
            if (task.Succeed)
            {
                Confirmed = true;
            }
            window.Close();
        }
    }
}
