using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Warehouse.Wpf.UI.Converters
{
    public class NdTotalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nd = value as double[];
            return nd == null ? 0 : nd.Sum();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
