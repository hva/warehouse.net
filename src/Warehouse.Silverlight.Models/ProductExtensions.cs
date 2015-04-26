using System;

namespace Warehouse.Silverlight.Models
{
    public static class ProductExtensions
    {
        public static long CalculatePriceRozn(string priceOpt, string k, string length, bool isSheet)
        {
            var _priceOpt = decimal.Parse(priceOpt);
            var _k = decimal.Parse(k);
            var rozn = _priceOpt * _k / 1000m * 1.2m;
            if (isSheet)
            {
                var _l = decimal.Parse(length);
                rozn *= _l;
            }
            return (long)(decimal.Ceiling(rozn / 100) * 100);
        }

        public static long CalculatePriceRozn(this Product p)
        {
            var priceOpt = Convert.ToString(p.PriceOpt);
            var k = Convert.ToString(p.K);
            var length = Convert.ToString(p.Length);
            return CalculatePriceRozn(priceOpt, k, length, p.IsSheet);
        }
    }
}
