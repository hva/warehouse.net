using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Warehouse.Server.Data;
using Warehouse.Server.Identity;
using Warehouse.Server.Models;
using Warehouse.Server.ViewModels;

namespace Warehouse.Server.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly IMongoContext context;

        public UsersController(ApplicationUserManager userManager, IMongoContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IEnumerable<User> Get()
        {
            return context.Users.FindAll();
        }

        [Route("api/users/{login}/changePassword")]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangePassword(string login, ChangePassword model)
        {
            if (model == null || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (userManager == null) return new HttpResponseMessage(HttpStatusCode.InternalServerError);

            var user = await userManager.FindByNameAsync(login);
            if (user == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            var res = await userManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (!res.Succeeded)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(res.Errors.FirstOrDefault())
                };
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
