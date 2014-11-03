using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Warehouse.Server.Identity;

namespace Warehouse.Server.Controllers
{
    public class UserController : Controller
    {
        private const string LoginError = "The password you entered is incorrect.<br />Please try again.";

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string login, string password, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Login = login;

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindAsync(login, password);
            if (user == null)
            {
                ViewBag.Error = LoginError;
                return View();
            }

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var userIdentity = await user.GenerateUserIdentityAsync(userManager);
            AuthenticationManager.SignIn(userIdentity);
            return RedirectToLocal(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}