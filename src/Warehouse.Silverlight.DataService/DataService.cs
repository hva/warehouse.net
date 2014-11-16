using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.DataService.Auth;
using Warehouse.Silverlight.DataService.Http;
using Warehouse.Silverlight.DataService.Infrastructure;
using Warehouse.Silverlight.Models;
using Warehouse.Silverlight.Navigation;

namespace Warehouse.Silverlight.DataService
{
    public class DataService : IDataService
    {
        private readonly IAuthService authService;
        private readonly INavigationService navigationService;

        public DataService(IAuthService authService, INavigationService navigationService)
        {
            this.authService = authService;
            this.navigationService = navigationService;
        }

        public async Task<AsyncResult<Product[]>> GetProductsAsync()
        {
            EnsureValidToken();
            using (var client = new BearerHttpClient(authService.Token.AccessToken))
            {
                var str = await client.GetStringAsync(new Uri("api/products", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<Product[]>(str);
                return new AsyncResult<Product[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<Product>> GetProductAsync(string id)
        {
            EnsureValidToken();
            using (var client = new BearerHttpClient(authService.Token.AccessToken))
            {
                var uri = new Uri(string.Concat("api/products/", id), UriKind.Relative);
                var str = await client.GetStringAsync(uri);
                var res = JsonConvert.DeserializeObject<Product>(str);
                return new AsyncResult<Product> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult> SaveProductAsync(Product product)
        {
            EnsureValidToken();
            using (var client = new BearerHttpClient(authService.Token.AccessToken))
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

        private void EnsureValidToken()
        {
            if (authService == null || !authService.IsValid())
            {
                navigationService.OpenLoginPage();
            }
        }
    }
}
