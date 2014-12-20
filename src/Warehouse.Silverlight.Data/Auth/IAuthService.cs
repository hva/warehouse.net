using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Data.Auth
{
    public interface IAuthService
    {
        bool IsAuthenticated();
        AuthToken Token { get; }
        Task<AsyncResult> Login(string login, string password);
        void Logout();
    }
}
