using System;
using System.Globalization;
using System.Linq;

namespace Warehouse.Silverlight.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public double K { get; set; }
        public int PriceOpt { get; set; }
        public int PriceRozn { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public double[] Nd { get; set; }
        public double Length { get; set; }
        public string Internal { get; set; }
        public long PriceIcome { get; set; }

        public string NdTotal
        {
            get
            {
                if (Nd == null || Nd.Length == 0)
                {
                    return null;
                }
                return Nd.Sum().ToString(CultureInfo.InvariantCulture);
            }
        }
    }
}
