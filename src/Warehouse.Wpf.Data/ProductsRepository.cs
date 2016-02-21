using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Data
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly Func<BearerHttpClient> httpClientFactory;

        public ProductsRepository(Func<BearerHttpClient> httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<AsyncResult<Product[]>> GetAsync()
        {
            using (var client = httpClientFactory())
            {
                var str = await client.GetStringAsync(new Uri("api/products", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<Product[]>(str);
                return new AsyncResult<Product[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<Product>> GetAsync(string id)
        {
            using (var client = httpClientFactory())
            {
                var uri = new Uri(string.Concat("api/products/", id), UriKind.Relative);
                var str = await client.GetStringAsync(uri);
                var res = JsonConvert.DeserializeObject<Product>(str);
                return new AsyncResult<Product> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<Product[]>> GetManyAsync(List<string> ids)
        {
            using (var client = httpClientFactory())
            {
                var data = JsonConvert.SerializeObject(ids);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var uri = new Uri("api/products/getMany", UriKind.Relative);
                    using (var resp = await client.PostAsync(uri, content))
                    {
                        var str = await resp.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<Product[]>(str);
                        return new AsyncResult<Product[]> { Result = res, Succeed = true };
                    }
                }
            }
        }

        public async Task<AsyncResult<ProductName[]>> GetNamesAsync()
        {
            using (var client = httpClientFactory())
            {
                var str = await client.GetStringAsync(new Uri("api/products/getNames", UriKind.Relative));
                var res = JsonConvert.DeserializeObject<ProductName[]>(str);
                return new AsyncResult<ProductName[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<string>> SaveAsync(Product product)
        {
            using (var client = httpClientFactory())
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
                            var str = await resp.Content.ReadAsStringAsync();
                            var id = JsonConvert.DeserializeObject<string>(str);
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

        public async Task<AsyncResult> UpdatePrice(ProductPriceUpdate[] prices)
        {
            var succeed = false;
            using (var client = httpClientFactory())
            {
                var data = JsonConvert.SerializeObject(prices);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var uri = new Uri("api/products/updatePrice", UriKind.Relative);
                    var resp = await client.PutAsync(uri, content);
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        succeed = true;
                    }
                }
            }
            return new AsyncResult { Succeed = succeed };
        }

        public async Task<AsyncResult> Delete(List<string> ids)
        {
            var succeed = false;
            using (var client = httpClientFactory())
            {
                var q = string.Join(",", ids);
                var uriString = string.Concat("api/products?ids=", q);
                var uri = new Uri(uriString, UriKind.Relative);
                var resp = await client.DeleteAsync(uri);
                if (resp.StatusCode == HttpStatusCode.OK)
                {
                    succeed = true;
                }
            }
            return new AsyncResult { Succeed = succeed };
        }
    }
}
