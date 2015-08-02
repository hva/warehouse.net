using System;

namespace Warehouse.Wpf.Infrastructure
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreKey(this string source, string search)
        {
            return source.IndexOf(search, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
