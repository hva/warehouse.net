using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver;
using Warehouse.Api.Data;
using Warehouse.Api.Data.Entities;
using Warehouse.Api.Data.Models;
using Warehouse.Api.Models;

namespace Warehouse.Api.Controllers
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

        public async Task<IHttpActionResult> Get()
        {
            var data = await context.Users
                .Find(new BsonDocument())
                .Project(x => new { x.UserName, x.Roles })
                .ToListAsync();

            return Ok(data);
        }

        public async Task<IHttpActionResult> Post([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User { UserName = model.UserName };
            var result = userManager.Create(user, model.Password);
            if (result.Succeeded)
            {
                var result2 = await userManager.AddUserToRolesAsync(user.Id, model.Roles);
                if (result2.Succeeded)
                {
                    return Created(string.Empty, user.UserName);
                }

                return BadRequest(result2.Errors.FirstOrDefault());
            }

            return BadRequest(result.Errors.FirstOrDefault());
        }

        public async Task<IHttpActionResult> Put([FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appUser = await userManager.FindByNameAsync(model.UserName);
            if (appUser == null)
            {
                return NotFound();
            }

            var res = await userManager.RemoveUserFromRolesAsync(appUser.Id, appUser.Roles);
            if (res.Succeeded)
            {
                var res2 = await userManager.AddUserToRolesAsync(appUser.Id, model.Roles);
                if (res2.Succeeded)
                {
                    return Ok();
                }
            }
            return InternalServerError();
        }

        [Route("api/users/{login}/changePassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword(string login, ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userManager.FindByNameAsync(login);
            if (user == null)
            {
                return NotFound();
            }

            var res = await userManager.ChangePasswordAsync(user.Id, model.OldPassword, model.NewPassword);
            if (!res.Succeeded)
            {
                return BadRequest(res.Errors.FirstOrDefault());
            }

            return Ok();
        }
    }
}
