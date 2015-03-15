using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Http;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.Data
{
    public class DataService : IDataService
    {
        private AuthToken token;

        private readonly IAuthStore authStore;
        private readonly INavigationService navigationService;

        public DataService(IAuthStore authStore, INavigationService navigationService)
        {
            this.authStore = authStore;
            this.navigationService = navigationService;
        }


        private bool EnsureValidToken()
        {
            token = authStore.LoadToken();

            if (token == null || !token.IsAuthenticated())
            {
                navigationService.OpenLoginPage();
                return false;
            }
            return true;
        }
    }
}
