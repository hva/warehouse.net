using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Warehouse.Server.Hubs
{
    public class ProductsHub : Hub
    {
        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public void RaiseProductUpdated(string id)
        {
            Clients.Others.OnProductUpdated(id);
        }
    }
}