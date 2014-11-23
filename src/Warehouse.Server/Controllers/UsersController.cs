using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.Owin;

namespace Warehouse.Server.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        [Route("api/users/{login}/changePassword")]
        [HttpPost]
        public async Task<HttpResponseMessage> ChangePassword(string login, ChangePassword model)
        {
            if (model == null || string.IsNullOrEmpty(model.OldPassword) || string.IsNullOrEmpty(model.NewPassword))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var userManager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
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

    public class ChangePassword
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
