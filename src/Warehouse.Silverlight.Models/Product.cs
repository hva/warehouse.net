using System.Linq;

namespace Warehouse.Silverlight.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public double K { get; set; }
        public long PriceOpt { get; set; }
        public long PriceRozn { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public double[] Nd { get; set; }
        public double Length { get; set; }
        public long PriceIcome { get; set; }
        public string Internal { get; set; }
        public bool IsSheet { get; set; }
        public string Firma { get; set; }

        public double NdTotal { get { return Nd == null ? 0 : Nd.Sum(); } }
    }
}
