namespace Warehouse.Silverlight.DataService.Infrastructure
{
    public class AsyncResult
    {
        public bool Success { get; set; }
    }

    public class AsyncResult<T> : AsyncResult
    {
        public T Result { get; set; }
    }
}
