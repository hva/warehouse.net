using System.Threading.Tasks;

namespace Warehouse.Silverlight.SignalR
{
    public interface ISignalRClient
    {
        Task StartAsync();
        Task EnsureConnection();
        void Stop();
    }
}
