using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Practices.Prism.Events;
using Warehouse.Silverlight.Infrastructure.Events;

namespace Warehouse.Silverlight.SignalR
{
    public class SignalRClient : ISignalRClient
    {
        private const string HubName = "ProductsHub";

        private readonly IHubProxy hubProxy;
        private readonly HubConnection connection;

        private readonly IEventAggregator eventAggregator;

        public SignalRClient(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;

            connection = new HubConnection(System.Windows.Browser.HtmlPage.Document.DocumentUri.ToString());
            hubProxy = connection.CreateHubProxy(HubName);

            hubProxy.On<string>(ProductUpdatedEvent.HubEventName, OnProductUpdatedRemote);
            hubProxy.On<List<string>>(ProductUpdatedBatchEvent.HubEventName, OnProductUpdatedBatchRemote);
            hubProxy.On<List<string>>(ProductDeletedBatchEvent.HubEventName, OnProductDeletedBatchRemote);
        }

        public async Task StartAsync()
        {
            SubscribeLocal();
            await connection.Start();
        }

        public async Task EnsureConnection()
        {
            if (connection.State == ConnectionState.Disconnected)
            {
                UnsubscribeLocal();
                await StartAsync();
            }
        }

        public void Stop()
        {
            UnsubscribeLocal();
            if (connection != null)
            {
                connection.Stop();
            }
        }

        private void SubscribeLocal()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Subscribe(OnProductUpdatedLocal);
            eventAggregator.GetEvent<ProductUpdatedBatchEvent>().Subscribe(OnProductUpdatedBatchLocal);
            eventAggregator.GetEvent<ProductDeletedBatchEvent>().Subscribe(OnProductDeletedBatchLocal);
        }

        private void UnsubscribeLocal()
        {
            eventAggregator.GetEvent<ProductUpdatedEvent>().Unsubscribe(OnProductUpdatedLocal);
            eventAggregator.GetEvent<ProductUpdatedBatchEvent>().Unsubscribe(OnProductUpdatedBatchLocal);
            eventAggregator.GetEvent<ProductDeletedBatchEvent>().Unsubscribe(OnProductDeletedBatchLocal);
        }

        #region ProductUpdated

        public void OnProductUpdatedLocal(ProductUpdatedEventArgs e)
        {
            if (e.FromRemote) return;

            // product updated locally
            // we need to notify other clients
            hubProxy.Invoke(ProductUpdatedEvent.HubMethodName, e.ProductId).Wait();
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

        #endregion

        #region ProductUpdatedBatch

        public void OnProductUpdatedBatchLocal(ProductUpdatedBatchEventArgs e)
        {
            if (e.FromRemote) return;

            // products updated locally
            // we need to notify other clients
            hubProxy.Invoke(ProductUpdatedBatchEvent.HubMethodName, e.ProductIds).Wait();
        }

        private void OnProductUpdatedBatchRemote(List<string> ids)
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                // products updated remotely
                // we need to notify local modules
                var e = new ProductUpdatedBatchEventArgs(ids, true);
                eventAggregator.GetEvent<ProductUpdatedBatchEvent>().Publish(e);
            });
        }

        #endregion

        #region ProductDeletedBatch

        public void OnProductDeletedBatchLocal(ProductDeletedBatchEventArgs e)
        {
            if (e.FromRemote) return;

            // products removed locally
            // we need to notify other clients
            hubProxy.Invoke(ProductDeletedBatchEvent.HubMethodName, e.ProductIds).Wait();
        }

        private void OnProductDeletedBatchRemote(List<string> ids)
        {
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                // products removed remotely
                // we need to notify local modules
                var e = new ProductDeletedBatchEventArgs(ids, true);
                eventAggregator.GetEvent<ProductDeletedBatchEvent>().Publish(e);
            });
        }

        #endregion
    }
}
