using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Warehouse.Wpf.Data.Http;
using Warehouse.Wpf.Data.Interfaces;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.Data
{
    public class FilesRepository : IFilesRepository
    {
        private readonly Func<BearerHttpClient> httpClientFactory;

        public FilesRepository(Func<BearerHttpClient> httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
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
    }
}
