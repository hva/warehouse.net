using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.Infrastructure.Events;

namespace Warehouse.Silverlight.SignalRModule
{
    public class SignalRClient : ISignalRClient
    {
        // remote consts
        private const string ProductsHub = "ProductsHub";
        private const string ProductUpdatedEvent = "OnProductUpdated";
        private const string RaiseProductUpdated = "RaiseProductUpdated";

        private readonly IHubProxy hubProxy;
        private readonly HubConnection connection;

        private readonly IEventAggregator eventAggregator;

        public SignalRClient(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            connection = new HubConnection(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString());
            hubProxy = connection.CreateHubProxy(ProductsHub);

            hubProxy.On<string>(ProductUpdatedEvent, OnProductUpdatedRemote);
        }

        public async Task StartAsync()
        {
            SubscribeLocal();
            await connection.Start();
        }

        public void Stop()
        {
            UnsubscribeLocal();
            if (connection != null)
            {
                connection.Stop();
            }
        }

        public void OnProductUpdatedLocal(ProductUpdatedEventArgs e)
        {
            if (e.FromRemote) return;

            // product updated locally
            // we need to notify other clients
            hubProxy.Invoke(RaiseProductUpdated, e.ProductId).Wait();
        }

        private void SubscribeLocal()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Subscribe(OnProductUpdatedLocal);
        }

        private void UnsubscribeLocal()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Unsubscribe(OnProductUpdatedLocal);
        }

        private void OnProductUpdatedRemote(string productId)
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                // product updated remotely
                // we need to notify local modules
                var e = new ProductUpdatedEventArgs(productId, true);
                eventAggregator.GetEvent<ProductUpdatedEvent>().Publish(e);
            });
        }
    }
}
