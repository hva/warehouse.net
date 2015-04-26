using System;
using Microsoft.Practices.Prism.ViewModel;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ChangePriceItem : NotificationObject
    {
        private long newPriceOpt;
        private long newPriceRozn;
        private readonly string k;
        private readonly string length;

        public ChangePriceItem(Product p)
        {
            Product = p;

            k = Convert.ToString(p.K);
            length = Convert.ToString(p.Length);
        }

        public Product Product { get; private set; }

        public long NewPriceOpt
        {
            get { return newPriceOpt; }
            set
            {
                if (newPriceOpt != value)
                {
                    newPriceOpt = value;
                    RaisePropertyChanged(() => NewPriceOpt);
                }
            }
        }

        public long NewPriceRozn
        {
            get { return newPriceRozn; }
            set
            {
                if (newPriceRozn != value)
                {
                    newPriceRozn = value;
                    RaisePropertyChanged(() => NewPriceRozn);
                }
            }
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
