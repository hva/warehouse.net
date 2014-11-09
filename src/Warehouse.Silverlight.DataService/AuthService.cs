using System.Threading.Tasks;
using Warehouse.Silverlight.DataService.Infrastructure;

namespace Warehouse.Silverlight.DataService
{
    public class AuthService : IAuthService
    {
        public bool IsValid()
        {
            return false;
        }

        public Task<AsyncResult> Login(string login, string password)
        {
            throw new System.NotImplementedException();
        }
    }
}
