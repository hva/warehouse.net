using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Infrastructure.Interfaces;

namespace Warehouse.Wpf.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthStore store;
        private readonly IApplicationSettings settings;

        public AuthService(IAuthStore store, IApplicationSettings settings)
        {
            this.store = store;
            this.settings = settings;
        }

        public async Task<AsyncResult> Login(string login, string password)
        {
            var result = new AsyncResult();
            using (var client = new HttpClient { BaseAddress = new Uri(settings.Endpoint, UriKind.Absolute) })
            {
                var data = new Dictionary<string, string>
                {
                    {"grant_type", "password"},
                    {"username", login},
                    {"password", password},
                };

                using (var content = new FormUrlEncodedContent(data))
                using (var resp = await client.PostAsync("/Token", content))
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = await resp.Content.ReadAsStreamAsync())
                        using (var streamReader = new StreamReader(stream))
                        using (var jsonReader = new JsonTextReader(streamReader))
                        {
                            var serializer = new JsonSerializer();
                            var token = serializer.Deserialize<AuthToken>(jsonReader);
                            if (token != null)
                            {
                                store.SaveToken(token);
                                result.Succeed = true;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
