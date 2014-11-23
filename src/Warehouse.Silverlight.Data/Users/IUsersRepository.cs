using System.Threading.Tasks;
using Warehouse.Silverlight.Data.Infrastructure;

namespace Warehouse.Silverlight.Data.Users
{
    public interface IUsersRepository
    {
        Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword);
    }
}
