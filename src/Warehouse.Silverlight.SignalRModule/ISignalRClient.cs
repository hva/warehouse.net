using System.Threading.Tasks;

namespace Warehouse.Silverlight.SignalRModule
{
    public interface ISignalRClient
    {
        Task StartAsync();
        void Stop();
    }
}
