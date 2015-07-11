using Microsoft.Practices.Prism.PubSubEvents;

namespace Warehouse.Wpf.Events
{
    public class ProductUpdatedEvent : PubSubEvent<ProductUpdatedEventArgs>
    {
        public static string HubEventName = "OnProductUpdated";
        public static string HubMethodName = "RaiseProductUpdated";
    }
}
