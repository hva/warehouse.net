using System.Collections.Generic;

namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductUpdatedBatchEventArgs : SignalREventArgs
    {
        public ProductUpdatedBatchEventArgs(List<string> productIds, bool fromRemote) : base(fromRemote)
        {
            ProductIds = productIds;
        }

        public List<string> ProductIds { get; private set; }
    }
}
