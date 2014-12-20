using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Http;
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

        public async Task<AsyncResult> SaveProductAsync(Product product)
        {
            if (!EnsureValidToken()) return new AsyncResult<Product>();
            using (var client = new BearerHttpClient(token.AccessToken))
            {
                client.BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var data = JsonConvert.SerializeObject(product);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var uri = new Uri(string.Concat("api/products/", product.Id), UriKind.Relative);
                    await client.PutAsync(uri, content);
                    return new AsyncResult { Succeed = true };
                }
            }
        }

        private bool EnsureValidToken()
        {
            token = authStore.Load();

            if (token == null || !token.IsAuthenticated())
            {
                navigationService.OpenLoginPage();
                return false;
            }
            return true;
        }
    }
}
