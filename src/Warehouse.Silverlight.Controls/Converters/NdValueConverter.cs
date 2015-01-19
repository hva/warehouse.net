using System;
using System.Globalization;
using System.Windows.Data;

namespace Warehouse.Silverlight.Controls.Converters
{
    public class NdValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double[] parts = value as double[];
            if (parts != null)
            {
                return string.Join("\n", parts);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
