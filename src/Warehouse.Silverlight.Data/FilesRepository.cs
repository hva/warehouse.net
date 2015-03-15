using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Warehouse.Silverlight.Auth;
using Warehouse.Silverlight.Data.Http;
using Warehouse.Silverlight.Data.Interfaces;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Data
{
    public class FilesRepository : IFilesRepository
    {
        private readonly IAuthStore authStore;

        public FilesRepository(IAuthStore authStore)
        {
            this.authStore = authStore;
        }

        public async Task<AsyncResult<string>> Create(Stream stream, string fileName, string contentType)
        {
            var token = authStore.LoadToken();
            using (var client = new BearerHttpClient(token.AccessToken))
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
