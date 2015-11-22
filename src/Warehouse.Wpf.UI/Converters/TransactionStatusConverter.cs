using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Warehouse.Wpf.UI.Converters
{
    public class TransactionStatusConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<string, string> map = new Dictionary<string, string>
        {
            ["Draft"] = "Черновик",
            ["ForSale"] = "Реализация",
            ["Reserved"] = "Резерв",
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = value as string;
            if (!string.IsNullOrEmpty(key))
            {
                string val;
                if (map.TryGetValue(key, out val))
                {
                    return val;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
