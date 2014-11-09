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
        private const string TokenFileName = "auth_token";

        private AuthToken token;
        private readonly ILogger logger;

        public AuthService(ILogger logger)
        {
            this.logger = logger;
        }

        public string AccessToken { get { return token.AccessToken; } }

        public bool IsValid()
        {
            // checking in-memory token
            if (token != null)
            {
                return IsValid(token);
            }

            TryLoadToken();

            // checking saved token
            if (token != null)
            {
                return IsValid(token);
            }

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
                using (var file = store.OpenFile(TokenFileName, FileMode.Create))
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

        private void TryLoadToken()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                using (var file = store.OpenFile(TokenFileName, FileMode.Open))
                using (var reader = new StreamReader(file))
                using (var jsonReader = new JsonTextReader(reader))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    token = serializer.Deserialize<AuthToken>(jsonReader);
                }
            }
            catch (Exception e)
            {
                logger.Log(e);
            }
        }

        private static bool IsValid(AuthToken token)
        {
            return token.Expires > DateTime.UtcNow;
        }
    }
}
