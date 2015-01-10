using System;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Auth
{
    public static class TokenExtensions
    {
        public static bool IsAuthenticated(this AuthToken token)
        {
            return token.Expires > DateTime.UtcNow;
        }

        public static bool IsAdmin(this AuthToken token)
        {
            return token.Role == UserRole.Admin;
        }
        
        public static bool IsEditor(this AuthToken token)
        {
            return token.Role == UserRole.Admin || token.Role == UserRole.Editor;
        }
    }
}
