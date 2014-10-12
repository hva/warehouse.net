using Microsoft.Owin;
using Owin;
using Warehouse.Server.Hubs;

[assembly: OwinStartup(typeof(Startup))]

namespace Warehouse.Server.Hubs
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
