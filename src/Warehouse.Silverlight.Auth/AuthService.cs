using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.Data.Http;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IAuthStore store;

        public AuthService(IAuthStore store)
        {
            this.store = store;
        }

        public async Task<AsyncResult> Login(string login, string password)
        {
            var result = new AsyncResult();
            using (var client = new BaseHttpClient())
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
                            JsonSerializer serializer = new JsonSerializer();
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
