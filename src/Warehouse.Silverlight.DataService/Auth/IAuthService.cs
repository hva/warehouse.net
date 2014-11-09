using System.Threading.Tasks;
using Warehouse.Silverlight.DataService.Infrastructure;

namespace Warehouse.Silverlight.DataService.Auth
{
    public interface IAuthService
    {
        bool IsValid();
        AuthToken Token { get; }
        Task<AsyncResult> Login(string login, string password);
        void Logout();
    }
}
