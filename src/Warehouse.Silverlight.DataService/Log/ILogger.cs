using System;

namespace Warehouse.Silverlight.DataService.Log
{
    public interface ILogger
    {
        void Log(Exception e);
    }
}
