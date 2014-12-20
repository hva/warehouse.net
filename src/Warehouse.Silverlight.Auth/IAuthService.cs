using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Auth
{
    public interface IAuthService
    {
        Task<AsyncResult> Login(string login, string password);
    }
}
