using Prism.Events;

namespace Warehouse.Wpf.Events
{
    public class ProductUpdatedEvent : PubSubEvent<ProductUpdatedEventArgs>
    {
        public static string HubEventName = "OnProductUpdated";
        public static string HubMethodName = "RaiseProductUpdated";
    }
}
