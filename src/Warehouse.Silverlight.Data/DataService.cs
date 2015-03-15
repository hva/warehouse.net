using System;
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
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.Data
{
    public class DataService : IDataService
    {
        private AuthToken token;

        private readonly IAuthStore authStore;
        private readonly INavigationService navigationService;

        public DataService(IAuthStore authStore, INavigationService navigationService)
        {
            this.authStore = authStore;
            this.navigationService = navigationService;
        }

        public async Task<AsyncResult<Product[]>> GetProductsAsync()
        {
            if (!EnsureValidToken()) return new AsyncResult<Product[]>();
            using (var client = new BearerHttpClient(token.AccessToken))
            {
                var str = await client.GetStringAsync(new Uri("api/products", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<Product[]>(str);
                return new AsyncResult<Product[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<Product>> GetProductAsync(string id)
        {
            if (!EnsureValidToken()) return new AsyncResult<Product>();
            using (var client = new BearerHttpClient(token.AccessToken))
            {
                var uri = new Uri(string.Concat("api/products/", id), UriKind.Relative);
                var str = await client.GetStringAsync(uri);
                var res = JsonConvert.DeserializeObject<Product>(str);
                return new AsyncResult<Product> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<string>> SaveProductAsync(Product product)
        {
            if (!EnsureValidToken()) return new AsyncResult<string>();

            using (var client = new BearerHttpClient(token.AccessToken))
            {
                var data = JsonConvert.SerializeObject(product);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    if (product.Id == null)
                    {
                        var uri = new Uri("api/products/", UriKind.Relative);
                        var resp = await client.PostAsync(uri, content);
                        if (resp.StatusCode == HttpStatusCode.Created)
                        {
                            var id = await resp.Content.ReadAsStringAsync();
                            return new AsyncResult<string> { Result = id, Succeed = true };
                        }
                    }
                    else
                    {
                        var uri = new Uri(string.Concat("api/products/", product.Id), UriKind.Relative);
                        var resp = await client.PutAsync(uri, content);
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            return new AsyncResult<string> { Result = product.Id, Succeed = true };
                        }
                    }
                }
            }
            return new AsyncResult<string>();
        }

        private bool EnsureValidToken()
        {
            token = authStore.LoadToken();

            if (token == null || !token.IsAuthenticated())
            {
                navigationService.OpenLoginPage();
                return false;
            }
            return true;
        }
    }
}
