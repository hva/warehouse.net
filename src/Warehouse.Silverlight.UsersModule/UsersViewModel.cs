using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.UsersModule
{
    public class UsersViewModel : NotificationObject, IRegionMemberLifetime
    {
        private readonly IUsersRepository usersRepository;
        private User[] users;
        private readonly InteractionRequest<CreateUserViewModel> createUserRequest;
        private readonly InteractionRequest<EditUserViewModel> editUserRequest;

        public UsersViewModel(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
            createUserRequest = new InteractionRequest<CreateUserViewModel>();
            editUserRequest = new InteractionRequest<EditUserViewModel>();

            CreateUserCommand = new DelegateCommand(CreateUser);
            EditUserCommand = new DelegateCommand<User>(EditUser);

            LoadData();
        }

        public User[] Users
        {
            get { return users; }
            set { users = value; RaisePropertyChanged(() => Users); }
        }

        public ICommand CreateUserCommand { get; private set; }
        public ICommand EditUserCommand { get; private set; }
        public IInteractionRequest CreateUserRequest { get { return createUserRequest; } }
        public IInteractionRequest EditUserRequest { get { return editUserRequest; } }

        public bool KeepAlive { get { return false; } }

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
            createUserRequest.Raise(new CreateUserViewModel(usersRepository), Callback);
        }

        private void EditUser(User user)
        {
            if (user.Roles.All(x => x != UserRole.Admin))
            {
                editUserRequest.Raise(new EditUserViewModel(usersRepository, user), Callback);
            }
        }

        private void Callback(Confirmation vm)
        {
            if (vm.Confirmed)
            {
                LoadData();
            }
        }
    }
}
