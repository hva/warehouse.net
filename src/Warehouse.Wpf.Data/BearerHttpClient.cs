using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Warehouse.Wpf.Auth.Interfaces;
using Warehouse.Wpf.Infrastructure.Interfaces;

namespace Warehouse.Wpf.Data
{
    public class BearerHttpClient : HttpClient
    {
        private const string Bearer = "Bearer";

        public BearerHttpClient(IAuthStore authStore, IApplicationSettings settings)
        {
            BaseAddress = new Uri(settings.Endpoint, UriKind.Absolute);

            var token = authStore.LoadToken();
            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, token.AccessToken);
        }
    }
}
