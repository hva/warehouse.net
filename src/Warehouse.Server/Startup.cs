using Microsoft.Owin;
using Owin;
using Warehouse.Server;

[assembly: OwinStartup(typeof(Startup))]

namespace Warehouse.Server
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
