using System.Net.Http.Headers;

namespace Warehouse.Silverlight.Data.Http
{
    public class BearerHttpClient : BaseHttpClient
    {
        private const string Bearer = "Bearer";

        public BearerHttpClient(string token)
        {
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token);
        }
    }
}
