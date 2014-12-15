using System;
using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Warehouse.Server.Data;

namespace Warehouse.Server.Identity
{
    public class ApplicationIdentityContext : IdentityContext, IDisposable
    {
        public ApplicationIdentityContext(IMongoContext context)
        {
            Users = context.Database.GetCollection<IdentityUser>("users");
            //Roles = context.Database.GetCollection<IdentityRole>("roles");
        }

        public void Dispose()
        {
        }

        #region Owin

        public static ApplicationIdentityContext Create(IdentityFactoryOptions<ApplicationIdentityContext> options, IOwinContext context)
        {
            var mongoContext = context.Get<MongoContext>();
            return new ApplicationIdentityContext(mongoContext);
        }

        #endregion
    }
}