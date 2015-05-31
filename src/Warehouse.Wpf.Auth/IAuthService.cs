using System.Threading.Tasks;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.Auth
{
    public interface IAuthService
    {
        Task<AsyncResult> Login(string login, string password);
    }
}
