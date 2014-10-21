using Microsoft.AspNet.SignalR;

namespace Warehouse.Server.Hubs
{
    public class ProductsHub : Hub
    {
        public void RaiseProductUpdated(string id)
        {
            Clients.Others.OnProductUpdated(id + "_server");
        }
    }
}