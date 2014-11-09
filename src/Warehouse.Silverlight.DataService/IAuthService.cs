﻿using System.Threading.Tasks;
using Warehouse.Silverlight.DataService.Infrastructure;

namespace Warehouse.Silverlight.DataService
{
    public interface IAuthService
    {
        bool IsValid();
        Task<AsyncResult> Login(string login, string password);
    }
}
