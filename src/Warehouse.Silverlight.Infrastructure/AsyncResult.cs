namespace Warehouse.Silverlight.Infrastructure
{
    public class AsyncResult
    {
        public bool Succeed { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AsyncResult<T> : AsyncResult
    {
        public T Result { get; set; }
    }
}
