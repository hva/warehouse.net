using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Data.Users
{
    public interface IUsersRepository
    {
        Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword);
    }
}
