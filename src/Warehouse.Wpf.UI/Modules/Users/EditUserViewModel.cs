using System;
using System.Collections.Generic;
using System.Windows.Input;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Converters;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.UI.Modules.Users
{
    public class EditUserViewModel : ValidationObject, IConfirmation, IInteractionRequestAware
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
        public IDictionary<string, string> Roles { get; private set; }
        public string Role { get; set; }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private async void Save()
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
                Close();
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
