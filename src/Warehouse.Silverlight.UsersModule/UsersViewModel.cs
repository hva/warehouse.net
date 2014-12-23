using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Data.Users;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.UsersModule
{
    public class UsersViewModel : NotificationObject, INavigationAware
    {
        private readonly IUsersRepository usersRepository;
        private User[] users;
        private readonly ICommand createUserCommand;
        private readonly InteractionRequest<UserEditViewModel> editUserRequest;

        public UsersViewModel(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
            createUserCommand = new DelegateCommand(CreateUser);
            editUserRequest = new InteractionRequest<UserEditViewModel>();
        }

        public User[] Users
        {
            get { return users; }
            set { users = value; RaisePropertyChanged(() => Users); }
        }

        public ICommand CreateUserCommand { get { return createUserCommand; } }
        public IInteractionRequest EditUserRequest { get { return editUserRequest; } }

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            LoadData();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion

        private async void LoadData()
        {
            var task = await usersRepository.GetUsers();
            if (task.Succeed)
            {
                Users = task.Result;
            }
        }

        private void CreateUser()
        {
            editUserRequest.Raise(new UserEditViewModel());
        }
    }
}
