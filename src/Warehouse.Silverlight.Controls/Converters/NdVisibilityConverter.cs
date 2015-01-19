using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Warehouse.Silverlight.Controls.Converters
{
    public class NdVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double[] parts = value as double[];
            if (parts != null && parts.Length > 1)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
