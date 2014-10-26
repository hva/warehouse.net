using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace Warehouse.Silverlight.SignalRModule
{
    public class SignalRModule : IModule
    {
        //[Dependency]
        //public IUnityContainer Container { get; set; }

        public void Initialize()
        {

            //var client = Container.Resolve<SignalRClient>();
            //await client.StartAsync();
        }
    }
}
