using Microsoft.Practices.Prism.PubSubEvents;

namespace Warehouse.Wpf.Events
{
    public class ProductDeletedBatchEvent : PubSubEvent<ProductDeletedBatchEventArgs>
    {
        public static string HubEventName = "OnProductDeletedBatch";
        public static string HubMethodName = "RaiseProductDeletedBatch";
    }
}
