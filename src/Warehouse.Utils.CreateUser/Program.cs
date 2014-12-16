using System;
using System.IO;
using System.Reflection;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Warehouse.Server.Data;
using Warehouse.Server.Identity;

namespace Warehouse.Utils.CreateUser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                string exeName = Path.GetFileName(codeBase);

                Console.WriteLine("usage: {0} <username> <password>", exeName);
                return;
            }

            var username = args[0];
            var password = args[1];

            var mongoContext = new MongoContext();
            var identityContext = new ApplicationIdentityContext(mongoContext);
            var store = new UserStore<ApplicationUser>(identityContext);
            var manager = new ApplicationUserManager(store);

            var user = new ApplicationUser { UserName = username };
            var result = manager.Create(user, password);
            if (result.Succeeded)
            {
                var roleResult = manager.AddUserToRolesAsync(user.Id, new[] {"admin"});
                roleResult.Wait();

                Console.WriteLine("user created!");
                Console.ReadKey();
                return;
            }

            foreach (var error in result.Errors)
            {
                Console.WriteLine(error);
            }
            Console.ReadKey();
        }
    }
}
