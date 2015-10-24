using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Warehouse.Api.Data.Entities;
using Warehouse.Api.Data.Models;

namespace Warehouse.Api.Data
{
    public class AuthRepository
    {
        private readonly ApplicationUserManager userManager;

        public AuthRepository(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUser(CreateUserModel userModel)
        {
            var user = new User
            {
                UserName = userModel.UserName
            };

            var result = await userManager.CreateAsync(user, userModel.Password);

            return result;
        }

        public Task<User> FindUser(string userName, string password)
        {
            return userManager.FindAsync(userName, password);
        }
    }
}