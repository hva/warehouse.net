using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Warehouse.Server.Hubs
{
    public class ProductsHub : Hub<IClient>
    {
        public override Task OnConnected()
        {
            var con = Context.ConnectionId;
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

    public interface IClient
    {
        void OnProductUpdated(string id);
    }
}