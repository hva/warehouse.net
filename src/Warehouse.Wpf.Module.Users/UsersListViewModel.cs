using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Users
{
    public class UsersListViewModel : BindableBase
    {
        private readonly IUsersRepository usersRepository;
        private User[] users;
        private bool isBusy;
        private readonly InteractionRequest<CreateUserViewModel> createUserRequest;
        private readonly InteractionRequest<EditUserViewModel> editUserRequest;

        public UsersListViewModel(IUsersRepository usersRepository)
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
            set { SetProperty(ref users, value); }
        }

        public ICommand CreateUserCommand { get; private set; }
        public ICommand EditUserCommand { get; private set; }
        public IInteractionRequest CreateUserRequest { get { return createUserRequest; } }
        public IInteractionRequest EditUserRequest { get { return editUserRequest; } }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private async void LoadData()
        {
            IsBusy = true;
            var task = await usersRepository.GetUsers();
            IsBusy = false;
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

        private void Callback(IConfirmation vm)
        {
            if (vm.Confirmed)
            {
                LoadData();
            }
        }
    }
}
