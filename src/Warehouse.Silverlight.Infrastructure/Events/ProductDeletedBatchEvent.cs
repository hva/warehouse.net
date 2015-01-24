using Microsoft.Practices.Prism.Events;

namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductDeletedBatchEvent : CompositePresentationEvent<ProductDeletedBatchEventArgs>
    {
        public static string HubEventName = "OnProductDeletedBatch";
        public static string HubMethodName = "RaiseProductDeletedBatch";
    }
}
