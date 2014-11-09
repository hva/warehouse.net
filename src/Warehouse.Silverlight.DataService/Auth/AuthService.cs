using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.DataService.Infrastructure;

namespace Warehouse.Silverlight.DataService.Auth
{
    public class AuthService : IAuthService
    {
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
                        StreamReader streamReader = new StreamReader(await resp.Content.ReadAsStreamAsync());
                        JsonReader reader = new JsonTextReader(streamReader);
                        JsonSerializer serializer = new JsonSerializer();
                        var token = serializer.Deserialize<AuthToken>(reader);
                        //using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                        //using (var file = store.OpenFile("auth", FileMode.Create))
                        //{
                        //    await resp.Content.CopyToAsync(file);
                        //}
                    }
                }
            }
            return result;
        }
    }
}
