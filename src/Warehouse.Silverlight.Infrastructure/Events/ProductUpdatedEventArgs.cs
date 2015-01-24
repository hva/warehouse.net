namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductUpdatedEventArgs : SignalREventArgs
    {
        public ProductUpdatedEventArgs(string productId, bool fromRemote) : base(fromRemote)
        {
            ProductId = productId;
        }

        public string ProductId { get; private set; }
    }
}
