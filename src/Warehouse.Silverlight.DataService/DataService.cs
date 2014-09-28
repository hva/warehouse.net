using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.DataService
{
    public class DataService : IDataService
    {
        public async Task<Product[]> GetProductsAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = System.Windows.Browser.HtmlPage.Document.DocumentUri;
                var str = await client.GetStringAsync(new Uri("api/products", UriKind.Relative));
                return JsonConvert.DeserializeObject<Product[]>(str);
            }
        }
    }
}
