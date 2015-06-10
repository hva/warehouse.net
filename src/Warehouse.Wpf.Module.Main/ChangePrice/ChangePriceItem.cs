using System;
using System.Globalization;
using Microsoft.Practices.Prism.Mvvm;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Module.Main.ChangePrice
{
    public class ChangePriceItem : BindableBase
    {
        private long newPriceOpt;
        private long newPriceRozn;
        private readonly string k;
        private readonly string length;

        public ChangePriceItem(Product p)
        {
            Product = p;

            k = Convert.ToString(p.K, CultureInfo.InvariantCulture);
            length = Convert.ToString(p.Length, CultureInfo.InvariantCulture);
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

            var priceOptStr = Convert.ToString(newPriceOpt);
            NewPriceRozn = ProductExtensions.CalculatePriceRozn(priceOptStr, k, length, Product.IsSheet);
        }
    }
}
