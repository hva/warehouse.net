using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.Data.Http;
using Warehouse.Silverlight.Data.Infrastructure;
using Warehouse.Silverlight.Data.Log;

namespace Warehouse.Silverlight.Data.Auth
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

        public AuthToken Token { get { return token; } }

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

        public void Logout()
        {
            // removing from memory
            token = null;

            // removing from storage
            TryRemoveToken();
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
                {
                    if (store.FileExists(TokenFileName))
                    {
                        using (var file = store.OpenFile(TokenFileName, FileMode.Open))
                        using (var reader = new StreamReader(file))
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            token = serializer.Deserialize<AuthToken>(jsonReader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(e);
            }
        }

        private void TryRemoveToken()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (store.FileExists(TokenFileName))
                    {
                        store.DeleteFile(TokenFileName);
                    }
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
