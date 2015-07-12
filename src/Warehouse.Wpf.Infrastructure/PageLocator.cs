using System;
using System.Collections.Generic;

namespace Warehouse.Wpf.Infrastructure
{
    public static class PageLocator
    {
        private static readonly Dictionary<string, Type> pages = new Dictionary<string, Type>();
        private static Action<object, object> openWindowCallback;

        public static void SetOpenWindowCallback(Action<object, object> callback)
        {
            openWindowCallback = callback;
        }

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

        public static void OpenWindow(string pageName, object param)
        {
            Type type;
            if (pages.TryGetValue(pageName, out type) && openWindowCallback != null)
            {
                var window = Activator.CreateInstance(type);
                openWindowCallback(window, param);
            }
        }
    }
}
