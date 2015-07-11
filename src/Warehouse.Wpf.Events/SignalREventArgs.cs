namespace Warehouse.Wpf.Events
{
    public abstract class SignalREventArgs
    {
        protected SignalREventArgs(bool fromRemote)
        {
            FromRemote = fromRemote;
        }

        public bool FromRemote { get; private set; }
    }
}
