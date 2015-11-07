using Warehouse.Wpf.Models;

namespace Warehouse.SharedModels
{
    public class MemoModel
    {
        public long PriceOpt { get; set; }
        public long PriceRozn { get; set; }
        public double Margin { get; set; }

        public Product Product { get; set; }
    }
}
