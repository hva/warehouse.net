using System;

namespace Warehouse.Silverlight.Data.Log
{
    public interface ILogger
    {
        void Log(Exception e);
    }
}
