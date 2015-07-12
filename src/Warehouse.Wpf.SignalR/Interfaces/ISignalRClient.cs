using System.Threading.Tasks;

namespace Warehouse.Wpf.SignalR.Interfaces
{
    public interface ISignalRClient
    {
        Task StartAsync();
        Task EnsureConnection();
        void Stop();
    }
}
