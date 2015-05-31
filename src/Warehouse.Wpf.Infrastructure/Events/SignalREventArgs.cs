﻿namespace Warehouse.Wpf.Infrastructure.Events
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
