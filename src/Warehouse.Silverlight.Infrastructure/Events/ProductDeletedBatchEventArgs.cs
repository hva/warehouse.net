namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductDeletedBatchEventArgs : SignalREventArgs
    {
        public ProductDeletedBatchEventArgs(string[] productIds, bool fromRemote) : base(fromRemote)
        {
            ProductIds = productIds;
        }

        public string[] ProductIds { get; private set; }
    }
}
