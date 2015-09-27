using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Warehouse.Api
{
    public static class OAuthConfig
    {
        public static void Configure(IAppBuilder app, IOAuthAuthorizationServerProvider authProvider)
        {
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = authProvider
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}