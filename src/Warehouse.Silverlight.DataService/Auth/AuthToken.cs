using System;
using Newtonsoft.Json;

namespace Warehouse.Silverlight.DataService.Auth
{
    public class AuthToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }
    }
}
