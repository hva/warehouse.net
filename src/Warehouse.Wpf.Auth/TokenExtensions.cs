using System;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.Auth
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
