using System;
using System.Globalization;
using System.Windows.Data;

namespace Warehouse.Silverlight.MainModule.Converters
{
    public class ZeroToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // from code to UI
            if (string.Equals(value, "0"))
            {
                return string.Empty;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // from UI to code
            if (string.Equals(value, string.Empty))
            {
                return "0";
            }
            return value;
        }
    }
}
