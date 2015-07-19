namespace Warehouse.Wpf.Models
{
    public static class ProductExtensions
    {
        public static long CalculatePriceRozn(long priceOpt, double k, double length, bool isSheet)
        {
            var _priceOpt = new decimal(priceOpt);
            var _k = new decimal(k);
            var rozn = _priceOpt * _k / 1000m * 1.2m;
            if (isSheet)
            {
                var _l = new decimal(length);
                rozn *= _l;
            }
            return (long)(decimal.Ceiling(rozn / 100) * 100);
        }

        public static long CalculatePriceRozn(this Product p)
        {
            return CalculatePriceRozn(p.PriceOpt, p.K, p.Length, p.IsSheet);
        }
    }
}
