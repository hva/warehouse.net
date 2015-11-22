using System;
using System.Globalization;
using System.Windows.Data;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.UI.Converters
{
    public class MemoMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var memo = value as MemoModel;
            if (memo != null)
            {
                var before = memo.Product.PriceOpt;
                var after = memo.PriceOpt;
                if (before > 0 && after > 0)
                {
                    return ProductExtensions.CalculateMargin(before, after);
                }
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
