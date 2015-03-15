using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Http;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IAuthStore authStore;

        public UsersRepository(IAuthStore authStore)
        {
            this.authStore = authStore;
        }

        public async Task<AsyncResult<User[]>> GetUsers()
        {
            var token = authStore.LoadToken();
            using (var client = new BearerHttpClient(token.AccessToken))
            {
                var str = await client.GetStringAsync(new Uri("api/users", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<User[]>(str);
                return new AsyncResult<User[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            var result = new AsyncResult();
            var token = authStore.LoadToken();
            using (var client = new BearerHttpClient(token.AccessToken))
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
                        result.ErrorMessage = TranslateError(err);
                    }
                    return result;
                }
            }
        }

        public async Task<AsyncResult> CreateUser(User user)
        {
            var result = new AsyncResult();
            var token = authStore.LoadToken();
            using (var client = new BearerHttpClient(token.AccessToken))
            {
                var data = JsonConvert.SerializeObject(user);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var uri = new Uri("api/users", UriKind.Relative);
                    using (var resp = await client.PostAsync(uri, content))
                    {
                        if (resp.StatusCode == HttpStatusCode.Created)
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
                            result.ErrorMessage = TranslateError(err);
                        }
                        return result;
                    }
                }
            }
        }

        public async Task<AsyncResult> UpdateUser(User user)
        {
            var result = new AsyncResult();
            var token = authStore.LoadToken();
            using (var client = new BearerHttpClient(token.AccessToken))
            {
                var data = JsonConvert.SerializeObject(user);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var uri = new Uri("api/users", UriKind.Relative);
                    using (var resp = await client.PutAsync(uri, content))
                    {
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            result.Succeed = true;
                        }
                    }
                }
            }
            return result;
        }

        public static string TranslateError(string err)
        {
            if (err.IndexOf("Incorrect password", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return "Прежний пароль введён неправильно";
            }
            if (err.IndexOf("Passwords must have at least one lowercase", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return "Пароль должен содержать хотя бы одну букву";
            }
            if (err.IndexOf("Passwords must have at least one digit", StringComparison.InvariantCultureIgnoreCase) > -1)
            {
                return "Пароль должен содержать хотя бы одну цифру";
            }
            return err;
        }
    }
}
