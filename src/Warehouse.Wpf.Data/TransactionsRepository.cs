using System;
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
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly Func<BearerHttpClient> httpClientFactory;
        private const string transactionsEndpoint = "api/transactions/";

        public TransactionsRepository(Func<BearerHttpClient> httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<AsyncResult<TransactionModel[]>> GetAsync()
        {
            using (var client = httpClientFactory())
            {
                var str = await client.GetStringAsync(new Uri(transactionsEndpoint, UriKind.Relative));
                var res = JsonConvert.DeserializeObject<TransactionModel[]>(str);
                return new AsyncResult<TransactionModel[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<string>> SaveAsync(TransactionModel transaction)
        {
            using (var client = httpClientFactory())
            {
                var data = JsonConvert.SerializeObject(transaction);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    if (transaction.Id == null)
                    {
                        var uri = new Uri(transactionsEndpoint, UriKind.Relative);
                        var resp = await client.PostAsync(uri, content);
                        if (resp.StatusCode == HttpStatusCode.Created)
                        {
                            var id = await resp.Content.ReadAsStringAsync();
                            return new AsyncResult<string> { Result = id, Succeed = true };
                        }
                    }
                    else
                    {
                        var uri = new Uri(string.Concat(transactionsEndpoint, transaction.Id), UriKind.Relative);
                        var resp = await client.PutAsync(uri, content);
                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            return new AsyncResult<string> { Result = transaction.Id, Succeed = true };
                        }
                    }
                }
            }
            return new AsyncResult<string>();
        }

        public async Task<AsyncResult> DeleteAsync(string[] ids)
        {
            var succeed = false;
            using (var client = httpClientFactory())
            {
                var q = string.Join(",", ids);
                var uriString = string.Concat(transactionsEndpoint, "?ids=", q);
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
