using System.Threading.Tasks;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Data.Interfaces
{
    public interface IUsersRepository
    {
        Task<AsyncResult<User[]>> GetUsers();
        Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword);
        Task<AsyncResult> CreateUser(User user);
        Task<AsyncResult> UpdateUser(User user);
    }
}
