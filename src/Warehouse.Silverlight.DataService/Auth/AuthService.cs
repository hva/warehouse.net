using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.DataService.Infrastructure;
using Warehouse.Silverlight.DataService.Log;

namespace Warehouse.Silverlight.DataService.Auth
{
    public class AuthService : IAuthService
    {
        private AuthToken token;
        private readonly ILogger logger;

        public AuthService(ILogger logger)
        {
            this.logger = logger;
        }

        public bool IsValid()
        {
            return false;
        }

        public async Task<AsyncResult> Login(string login, string password)
        {
            var result = new AsyncResult();
            using (var client = new DataServiceHttpClient())
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
                            token = serializer.Deserialize<AuthToken>(jsonReader);
                            if (token != null)
                            {
                                TrySaveToken();
                                result.Succeed = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        private void TrySaveToken()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                using (var file = store.OpenFile("auth", FileMode.Create))
                using (var writer = new StreamWriter(file))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, token);
                }
            }
            catch (Exception e)
            {
                logger.Log(e);
            }
        }
    }
}
