using System;
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;
using Warehouse.Silverlight.Log;

namespace Warehouse.Silverlight.Auth
{
    public class AuthStore : IAuthStore
    {
        private const string TokenFileName = "auth_token";
        private AuthToken inMemoryToken;
        private readonly ILogger logger;

        public AuthStore(ILogger logger)
        {
            this.logger = logger;
        }

        public void SaveToken(AuthToken token)
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
                inMemoryToken = token;
            }
        }

        public AuthToken LoadToken()
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
                            return serializer.Deserialize<AuthToken>(jsonReader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(e);
            }
            return inMemoryToken;
        }

        public void ClearToken()
        {
            inMemoryToken = null;
            TryRemoveToken();
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
    }
}
