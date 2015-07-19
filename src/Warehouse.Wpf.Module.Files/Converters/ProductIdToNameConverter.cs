using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Warehouse.Wpf.Module.Files.Converters
{
    public class ProductIdToNameConverter : IValueConverter
    {
        private static readonly Dictionary<string, string> names = new Dictionary<string, string>();

        public static void SetNames(IDictionary<string, string> items)
        {
            if (items != null)
            {
                foreach (var x in items)
                {
                    names[x.Key] = x.Value;
                }
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ids = value as string[];
            if (ids != null)
            {
                return string.Join(", ", ids.Select(ResolveName));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string ResolveName(string id)
        {
            string name;
            if (names.TryGetValue(id, out name))
            {
                return name;
            }
            return null;
        }
    }
}
