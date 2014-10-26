namespace Warehouse.Silverlight.Infrastructure.Events
{
    public class ProductUpdatedEventArgs
    {
        public ProductUpdatedEventArgs(string productId) : this(productId, false)
        {
        }

        public ProductUpdatedEventArgs(string productId, bool fromRemote)
        {
            ProductId = productId;
            FromRemote = fromRemote;
        }

        public string ProductId { get; private set; }
        public bool FromRemote { get; private set; }
    }
}
