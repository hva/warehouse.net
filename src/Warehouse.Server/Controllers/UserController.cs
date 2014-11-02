using System.Web.Mvc;

namespace Warehouse.Server.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string login, string password)
        {
            return View();
        }
    }
}