using System;
using System.IO;
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
    public class FilesRepository : IFilesRepository
    {
        private readonly Func<BearerHttpClient> httpClientFactory;

        public FilesRepository(Func<BearerHttpClient> httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<AsyncResult<FileDescription[]>> GetAll()
        {
            using (var client = httpClientFactory())
            {
                var uri = new Uri("api/files", UriKind.Relative);
                var str = await client.GetStringAsync(uri);
                var res = JsonConvert.DeserializeObject<FileDescription[]>(str);
                return new AsyncResult<FileDescription[]> { Result = res, Succeed = true };
            }
        }

        public async Task<AsyncResult<string>> Create(Stream stream, string fileName, string contentType)
        {
            using (var client = httpClientFactory())
            using (var multipart = new MultipartFormDataContent())
            using (var content = new StreamContent(stream))
            {
                multipart.Add(content, contentType, fileName);
                var uri = new Uri("api/files", UriKind.Relative);
                var resp = await client.PostAsync(uri, multipart);
                if (resp.StatusCode == HttpStatusCode.Created)
                {
                    var id = await resp.Content.ReadAsStringAsync();
                    return new AsyncResult<string> { Result = id, Succeed = true };
                }
            }
            return new AsyncResult<string>();
        }

        public async Task<AsyncResult> AttachProducts(string fileId, string[] productIds)
        {
            using (var client = httpClientFactory())
            {
                var uriString = string.Format("api/files/{0}/products", fileId);
                var uri = new Uri(uriString, UriKind.Relative);
                var data = JsonConvert.SerializeObject(productIds);
                using (var content = new StringContent(data, Encoding.UTF8, "application/json"))
                {
                    var resp = await client.PostAsync(uri, content);
                    if (resp.StatusCode == HttpStatusCode.Created)
                    {
                        return new AsyncResult { Succeed = true };
                    }
                }
            }
            return new AsyncResult { Succeed = false };
        }

        public async Task<AsyncResult> DeleteAsync(string[] fileIds)
        {
            using (var client = httpClientFactory())
            {
                var q = string.Join(",", fileIds);
                var uriString = string.Format("api/files?ids={0}", q);
                var uri = new Uri(uriString, UriKind.Relative);
                var resp = await client.DeleteAsync(uri);
                return new AsyncResult { Succeed = resp.StatusCode == HttpStatusCode.OK };
            }
        }
    }
}
