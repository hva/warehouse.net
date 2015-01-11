using Microsoft.Practices.Prism.ViewModel;

namespace Warehouse.Silverlight.MainModule.ViewModels
{
    public class ChangePriceItem : NotificationObject
    {
        private long newPrice;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public long PriceOpt { get; set; }

        public long NewPrice
        {
            get { return newPrice; }
            set
            {
                if (newPrice != value)
                {
                    newPrice = value;
                    RaisePropertyChanged(() => NewPrice);
                }
            }
        }

        public void Refresh(double percentage)
        {
            var a = new decimal(PriceOpt);
            var x = new decimal(percentage);
            var b = a * (1 + x / 100);

            NewPrice = (long)(decimal.Ceiling(b / 100) * 100);
        }
    }
}
