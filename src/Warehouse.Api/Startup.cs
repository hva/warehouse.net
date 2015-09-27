using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Practices.Unity;

[assembly: OwinStartup(typeof(Warehouse.Api.Startup))]
namespace Warehouse.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            UnityConfig.Register(config);

            OAuthConfig.Configure(app, UnityConfig.Container.Resolve<IOAuthAuthorizationServerProvider>());

            WebApiConfig.Register(config);
            app.UseWebApi(config);

            app.MapSignalR();
        }
    }
}
