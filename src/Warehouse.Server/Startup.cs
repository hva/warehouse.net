﻿using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.DataProtection;
using Owin;
using Warehouse.Server;
using Warehouse.Server.Identity;
using Microsoft.Practices.Unity;

[assembly: OwinStartup(typeof(Startup))]
namespace Warehouse.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuthInternal(app);
            app.MapSignalR();
        }

        private static void ConfigureAuthInternal(IAppBuilder app)
        {
            var container = UnityConfig.GetConfiguredContainer();

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IdentityContext, ApplicationIdentityContext>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationUserManager>(new HierarchicalLifetimeManager());
            container.RegisterInstance(app.GetDataProtectionProvider(), new HierarchicalLifetimeManager());

            var provider = container.Resolve<ApplicationOAuthProvider>();
            app.ConfigureAuth(provider);
        }
    }
}
