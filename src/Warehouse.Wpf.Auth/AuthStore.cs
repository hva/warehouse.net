using System;
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace Warehouse.Wpf.Auth
{
    public class AuthStore : IAuthStore
    {
        private const string TokenFileName = "auth_token";
        private AuthToken inMemoryToken;
        //private readonly ILoggerFacade logger;

        public AuthStore(/*ILoggerFacade logger*/)
        {
            //this.logger = logger;
        }

        public void SaveToken(AuthToken token)
        {
            try
            {
                using (var store = GetStore())
                using (var file = store.OpenFile(TokenFileName, FileMode.Create))
                using (var writer = new StreamWriter(file))
                {
                    var serializer = new JsonSerializer();
                    serializer.Serialize(writer, token);
                }
            }
            catch (Exception e)
            {
                //logger.Log(e.Message, Category.Exception, Priority.None);
                inMemoryToken = token;
            }
        }

        public AuthToken LoadToken()
        {
            try
            {
                using (var store = GetStore())
                {
                    if (store.FileExists(TokenFileName))
                    {
                        using (var file = store.OpenFile(TokenFileName, FileMode.Open))
                        using (var reader = new StreamReader(file))
                        using (var jsonReader = new JsonTextReader(reader))
                        {
                            var serializer = new JsonSerializer();
                            return serializer.Deserialize<AuthToken>(jsonReader);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //logger.Log(e.Message, Category.Exception, Priority.None);
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
                //logger.Log(e.Message, Category.Exception, Priority.None);
            }
        }

        private static IsolatedStorageFile GetStore()
        {
            const IsolatedStorageScope scope = IsolatedStorageScope.User | IsolatedStorageScope.Assembly | IsolatedStorageScope.Domain;
            return IsolatedStorageFile.GetStore(scope, typeof(System.Security.Policy.Url), typeof(System.Security.Policy.Url));
        }
    }
}
