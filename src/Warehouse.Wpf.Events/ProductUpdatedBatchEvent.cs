using Microsoft.Practices.Prism.PubSubEvents;

namespace Warehouse.Wpf.Events
{
    public class ProductUpdatedBatchEvent : PubSubEvent<ProductUpdatedBatchEventArgs>
    {
        public static string HubEventName = "OnProductUpdatedBatch";
        public static string HubMethodName = "RaiseProductUpdatedBatch";
    }
}
