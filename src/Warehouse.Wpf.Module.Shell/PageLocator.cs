using System;
using System.Collections.Generic;

namespace Warehouse.Wpf.Module.Shell
{
    public static class PageLocator
    {
        private static readonly Dictionary<string, Type> pages = new Dictionary<string, Type>();

        public static void Register<T>(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                pages.Add(pageName, typeof(T));
            }
        }

        public static object Resolve(string pageName)
        {
            Type type;
            if (pages.TryGetValue(pageName, out type))
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
