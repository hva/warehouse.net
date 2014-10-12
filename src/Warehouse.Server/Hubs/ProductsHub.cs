using Microsoft.AspNet.SignalR;

namespace Warehouse.Server.Hubs
{
    public class ProductsHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}