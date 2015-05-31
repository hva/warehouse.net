using Microsoft.Practices.Prism.PubSubEvents;

namespace Warehouse.Wpf.Infrastructure.Events
{
    public class ProductUpdatedBatchEvent : PubSubEvent<ProductUpdatedBatchEventArgs>
    {
        public static string HubEventName = "OnProductUpdatedBatch";
        public static string HubMethodName = "RaiseProductUpdatedBatch";
    }
}
