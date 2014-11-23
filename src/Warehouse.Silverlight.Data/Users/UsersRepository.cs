using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Warehouse.Silverlight.Data.Auth;
using Warehouse.Silverlight.Data.Http;
using Warehouse.Silverlight.Data.Infrastructure;

namespace Warehouse.Silverlight.Data.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IAuthService authService;

        public UsersRepository(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            var result = new AsyncResult();
            using (var client = new BearerHttpClient(authService.Token.AccessToken))
            {
                var data = new Dictionary<string, string>
                {
                    {"oldPassword", oldPassword},
                    {"newPassword", newPassword},
                };

                var uri = string.Format("/api/users/{0}/changePassword", login);

                using (var content = new FormUrlEncodedContent(data))
                using (var resp = await client.PostAsync(uri, content))
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        result.Succeed = true;
                        return result;
                    }

                    if (resp.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        result.ErrorMessage = "Произошла ошибка на сервере";
                    }
                    else if (resp.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var err = await resp.Content.ReadAsStringAsync();
                        if (err.IndexOf("Incorrect password", StringComparison.InvariantCultureIgnoreCase) > -1)
                        {
                            result.ErrorMessage = "Прежний пароль введён неправильно";
                        }
                        // handle other errors
                    }
                    return result;
                }
            }
        }
    }
}
