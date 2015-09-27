using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Unity.WebApi;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Providers;

namespace Warehouse.Api
{
    public static class UnityConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Container = new UnityContainer();

            Container.RegisterType<IMongoContext, MongoContext>(new HierarchicalLifetimeManager());
            Container.RegisterType<IOAuthAuthorizationServerProvider, SimpleAuthorizationServerProvider>();

            config.DependencyResolver = new UnityDependencyResolver(Container);
        }

        public static IUnityContainer Container { get; private set; }
    }
}