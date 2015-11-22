using System;
using System.Globalization;
using System.Windows.Data;

namespace Warehouse.Wpf.UI.Converters
{
    public class RelativeTimeConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateTime = System.Convert.ToDateTime(value);
            var span = DateTime.UtcNow.Subtract(dateTime);

            if (span.Days > 7)
            {
                return dateTime.ToLocalTime().ToString("d MMMM HH:mm");
            }
            if (span.Hours > 24)
            {
                return dateTime.ToLocalTime().ToString("dddd HH:mm");
            }
            return dateTime.ToLocalTime().ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
