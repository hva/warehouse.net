using System;
using System.Net;
using System.Net.Browser;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.DataService.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.DataService
{
    public class DataService : IDataService
    {
        public DataService()
        {
            WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
        }

        public async Task<AsyncResult<Product[]>> GetProductsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var str = await client.GetStringAsync(new Uri("api/products", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<Product[]>(str);
                return new AsyncResult<Product[]> { Result = res, Success = true };
            }
        }

        public async Task<AsyncResult> SaveProductAsync(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var data = JsonConvert.SerializeObject(product);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var uri = new Uri(string.Concat("api/products/", product.Id), UriKind.Relative);
                    await client.PutAsync(uri, content);
                    return new AsyncResult { Success = true };
                }
            }
        }
    }
}
