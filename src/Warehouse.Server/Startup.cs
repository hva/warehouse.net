using Microsoft.Owin;
using Owin;
using Warehouse.Server;

[assembly: OwinStartup(typeof(Startup))]
namespace Warehouse.Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
