﻿using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.Data.Users
{
    public interface IUsersRepository
    {
        Task<AsyncResult<User[]>> GetUsers();
        Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword);
        Task<AsyncResult> CreateUser(User user);
    }
}
