using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly Func<BearerHttpClient> httpClientFactory;

        public UsersRepository(Func<BearerHttpClient> httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<AsyncResult<User[]>> GetUsers()
        {
            using (var client = httpClientFactory())
            {
                var str = await client.GetStringAsync(new Uri("api/users", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<User[]>(str);
                return new AsyncResult<User[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult> ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            var result = new AsyncResult();
            using (var client = httpClientFactory())
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
            using (var client = httpClientFactory())
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
            using (var client = httpClientFactory())
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
