using System.Threading.Tasks;

namespace Warehouse.Wpf.SignalR
{
    public interface ISignalRClient
    {
        Task StartAsync();
        Task EnsureConnection();
        void Stop();
    }
}
