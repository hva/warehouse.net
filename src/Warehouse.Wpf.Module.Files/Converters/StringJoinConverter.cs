using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Warehouse.Wpf.Module.Files.Converters
{
    public class StringJoinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var items = value as IEnumerable<string>;
            if (items != null)
            {
                return string.Join(Environment.NewLine, items);
            }
            return null;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
