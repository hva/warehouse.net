using System.Net.Http;
using System.Net.Http.Headers;

namespace Warehouse.Wpf.Data.Http
{
    public class BearerHttpClient : HttpClient
    {
        private const string Bearer = "Bearer";

        public BearerHttpClient(string token)
        {
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token);
        }
    }
}
