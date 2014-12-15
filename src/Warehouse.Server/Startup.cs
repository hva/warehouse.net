using Microsoft.Owin;
using Owin;
using Warehouse.Server;
using Warehouse.Server.Identity;

[assembly: OwinStartup(typeof(Startup))]
namespace Warehouse.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.ConfigureAuth();
            app.MapSignalR();
        }
    }
}
