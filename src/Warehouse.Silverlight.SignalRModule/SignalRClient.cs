using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Warehouse.Silverlight.SignalRModule
{
    public class SignalRClient
    {
        public SignalRClient()
        {
            
        }

        public async Task StartAsync()
        {
            var hubConnection = new HubConnection(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString());
            IHubProxy stockTickerHubProxy = hubConnection.CreateHubProxy("ProductsHub");
            stockTickerHubProxy.On<string>("OnProductUpdated", id =>
            {

            });
            await hubConnection.Start();
            await stockTickerHubProxy.Invoke("RaiseProductUpdated", "123123123");
        }
    }
}
