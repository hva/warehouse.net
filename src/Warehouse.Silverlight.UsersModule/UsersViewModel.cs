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

        public User[] Users
        {
            get { return users; }
            set { users = value; RaisePropertyChanged(() => Users); }
        }

        public UsersViewModel(IUsersRepository usersRepository)
        {
            this.usersRepository = usersRepository;
        }

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
    }
}
