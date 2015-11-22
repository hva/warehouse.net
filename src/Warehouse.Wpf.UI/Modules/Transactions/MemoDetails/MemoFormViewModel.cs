using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.UI.Modules.Transactions.MemoDetails
{
    public class MemoFormViewModel : ValidationObject
    {
        private string priceOpt;
        private long priceRozn;
        private double margin;

        public MemoFormViewModel(MemoModel m)
        {
            Memo = m;
            PriceOpt = m.PriceOpt.ToString();
        }

        public MemoFormViewModel(Product p)
        {
            Memo = new MemoModel { Product = p };
            PriceOpt = p.PriceOpt.ToString();
        }

        public MemoModel Memo { get; }

        public bool IsValid()
        {
            ValidatePriceOpt();
            return !HasErrors;
        }

        public MemoModel GetUpdatedMemo()
        {
            Memo.PriceOpt = long.Parse(priceOpt);
            Memo.PriceRozn = priceRozn;
            return Memo;
        }

        #region PriceOpt

        public string PriceOpt
        {
            get { return priceOpt; }
            set
            {
                if (SetProperty(ref priceOpt, value))
                {
                    priceOpt = value;
                    ValidatePriceOpt();
                    UpdatePriceRozn();
                    UpdateMargin();
                }
            }
        }

        private void ValidatePriceOpt()
        {
            errorsContainer.ClearErrors(() => PriceOpt);
            errorsContainer.SetErrors(() => PriceOpt, Validate.Long(PriceOpt));
        }

        #endregion

        #region PriceRozn

        public long PriceRozn
        {
            get { return priceRozn; }
            set { SetProperty(ref priceRozn, value); }
        }

        protected void UpdatePriceRozn()
        {
            if (errorsContainer.HasErrors(() => PriceOpt))
            {
                PriceRozn = 0;
            }
            else
            {
                var _priceOpt = long.Parse(priceOpt);
                var k = Memo.Product.K;
                var length = Memo.Product.Length;
                var isSheet = Memo.Product.IsSheet;
                PriceRozn = ProductExtensions.CalculatePriceRozn(_priceOpt, k, length, isSheet);
            }
        }

        #endregion

        #region Margin

        public double Margin
        {
            get { return margin; }
            set { SetProperty(ref margin, value); }
        }

        private void UpdateMargin()
        {
            if (errorsContainer.HasErrors(() => PriceOpt) || Memo.Product.PriceOpt == 0)
            {
                Margin = 0;
            }
            else
            {
                Margin = ProductExtensions.CalculateMargin(Memo.Product.PriceOpt, long.Parse(priceOpt));
            }
        }

        #endregion
    }
}
