using Microsoft.Practices.Prism.Events;

namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductUpdatedEvent : CompositePresentationEvent<ProductUpdatedEventArgs>
    {
        public static string HubEventName = "OnProductUpdated";
        public static string HubMethodName = "RaiseProductUpdated";
    }
}
