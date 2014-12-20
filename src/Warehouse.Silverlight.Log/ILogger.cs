using System;

namespace Warehouse.Silverlight.Log
{
    public interface ILogger
    {
        void Log(Exception e);
    }
}
