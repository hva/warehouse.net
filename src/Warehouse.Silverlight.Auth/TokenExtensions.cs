using System;

namespace Warehouse.Silverlight.Auth
{
    public static class TokenExtensions
    {
        public static bool IsAuthenticated(this AuthToken token)
        {
            return token.Expires > DateTime.UtcNow;
        }
    }
}
