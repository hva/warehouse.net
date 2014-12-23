using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Warehouse.Silverlight.Controls.Converters
{
    public class RoleToStringConverter : IValueConverter
    {
        private static readonly IDictionary<string, string> tr = new Dictionary<string, string>
        {
            {"admin", "Администратор"},
            {"editor", "Менеджер"},
            {"user", "Кладовщик"},
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var roles = value as string[];
            if (roles != null && roles.Length > 0)
            {
                return Convert(roles[0]);
            }

            var role = value as string;
            if (role != null)
            {
                return Convert(role);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static string Convert(string key)
        {
            string val;
            return tr.TryGetValue(key, out val) ? val : key;
        }
    }
}
