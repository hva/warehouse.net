using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.ChangePrice
{
    public class ChangePriceItem : BindableBase
    {
        private long newPriceOpt;
        private long newPriceRozn;

        public ChangePriceItem(Product p)
        {
            Product = p;
        }

        public Product Product { get; private set; }

        public long NewPriceOpt
        {
            get { return newPriceOpt; }
            set { SetProperty(ref newPriceOpt, value); }
        }

        public long NewPriceRozn
        {
            get { return newPriceRozn; }
            set { SetProperty(ref newPriceRozn, value); }
        }

        public void Refresh(double percentage)
        {
            var a = new decimal(Product.PriceOpt);
            var x = new decimal(percentage);
            var b = a * (1 + x / 100);

            NewPriceOpt = (long)(decimal.Ceiling(b / 100) * 100);

            NewPriceRozn = ProductExtensions.CalculatePriceRozn(newPriceOpt, Product.K, Product.Length, Product.IsSheet);
        }
    }
}
