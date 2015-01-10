using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Controls.Converters
{
    public class RoleToStringConverter : IValueConverter
    {
        public static readonly IDictionary<string, string> RoleTranslations = new Dictionary<string, string>
        {
            { UserRole.Admin, "Администратор" },
            { UserRole.Editor, "Зав. складом" },
            { UserRole.User, "Менеджер" },
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
            return RoleTranslations.TryGetValue(key, out val) ? val : key;
        }
    }
}
