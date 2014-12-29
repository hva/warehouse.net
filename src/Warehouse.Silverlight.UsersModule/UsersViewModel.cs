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
        private readonly InteractionRequest<CreateUserViewModel> createUserRequest;

        public UsersViewModel(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
            CreateUserCommand = new DelegateCommand(CreateUser);
            createUserRequest = new InteractionRequest<CreateUserViewModel>();
        }

        public User[] Users
        {
            get { return users; }
            set { users = value; RaisePropertyChanged(() => Users); }
        }

        public ICommand CreateUserCommand { get; private set; }
        public IInteractionRequest CreateUserRequest { get { return createUserRequest; } }

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
            createUserRequest.Raise(new CreateUserViewModel(usersRepository), CreateUserCallback);
        }

        private void CreateUserCallback(CreateUserViewModel vm)
        {
            if (vm.Confirmed)
            {
                LoadData();
            }
        }
    }
}
