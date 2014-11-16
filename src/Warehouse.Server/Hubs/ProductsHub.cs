using Microsoft.AspNet.SignalR;

namespace Warehouse.Server.Hubs
{
    public class ProductsHub : Hub<IClient>
    {
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