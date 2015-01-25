using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Warehouse.Silverlight.MainModule.Models;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.Converters
{
    public class ProductFirmaToBrushConverter : IValueConverter
    {
        private readonly ProductFirmaMapper mapper = new ProductFirmaMapper();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var p = value as Product;
            if (p != null && p.Firma != null)
            {
                ProductFirma pf;
                if (mapper.TryGetValue(p.Firma, out pf))
                {
                    return new SolidColorBrush(pf.Color) { Opacity = 0.8 };
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
