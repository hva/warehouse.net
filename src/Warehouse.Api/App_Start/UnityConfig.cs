using System.Configuration;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.WebApi;
using Warehouse.Api.Data;
using Warehouse.Api.Providers;

namespace Warehouse.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Container = new UnityContainer();

            var connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;

            Container.RegisterType<IMongoContext, MongoContext>(new HierarchicalLifetimeManager(), new InjectionConstructor(connectionString));
            Container.RegisterType<IOAuthAuthorizationServerProvider, SimpleAuthorizationServerProvider>();

            config.DependencyResolver = new UnityDependencyResolver(Container);
        }

        public static IUnityContainer Container { get; private set; }
    }
}