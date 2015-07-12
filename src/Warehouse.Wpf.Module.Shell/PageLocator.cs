using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm;

namespace Warehouse.Wpf.Module.Shell
{
    public static class PageLocator
    {
        private static readonly Dictionary<string, Type> pages = new Dictionary<string, Type>();

        public static void Register<T>(string pageName) where T : IView
        {
            if (!string.IsNullOrEmpty(pageName))
            {
                pages.Add(pageName, typeof(T));
            }
        }

        public static IView Resolve(string pageName)
        {
            Type type;
            if (pages.TryGetValue(pageName, out type))
            {
                return (IView)Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
