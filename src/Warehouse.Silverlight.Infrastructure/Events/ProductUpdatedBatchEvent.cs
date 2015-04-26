using Microsoft.Practices.Prism.Events;

namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductUpdatedBatchEvent : CompositePresentationEvent<ProductUpdatedBatchEventArgs>
    {
        public static string HubEventName = "OnProductUpdatedBatch";
        public static string HubMethodName = "RaiseProductUpdatedBatch";
    }
}
