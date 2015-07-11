using System;
using System.Linq;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;
using Warehouse.Wpf.Mvvm;

namespace Warehouse.Wpf.Module.ProductDetail.Form
{
    public class SheetFormViewModel : ProductFormViewModel
    {
        private double[] sheetSizes;

        public SheetFormViewModel(Product product, bool canEditPrice)
            : base(product, canEditPrice)
        {
        }

        public override string LenghtLabel { get { return "Площадь листа (м²)"; } }
        public override bool IsLengthReadonly { get { return true; } }
        public override string NdLabel  { get { return "Н/Д (м²)"; } }

        #region Size

        public override string Size
        {
            get { return size; }
            set
            {
                if (size != value)
                {
                    size = value;
                    ValidateSize();
                    UpdateSheetLength();
                    UpdatePriceRozn();
                }
            }
        }

        protected override void ValidateSize()
        {
            errorsContainer.ClearErrors(() => Size);
            errorsContainer.SetErrors(() => Size, Validate.Required(Size));
            sheetSizes = ParseSheetSize(size);
            if (sheetSizes == null)
            {
                errorsContainer.SetErrors(() => Size, new[] { "строка в формате\nтолщина*ширина*длина" });
            }
        }

        #endregion

        protected override bool GetIsSheet()
        {
            return true;
        }

        private void UpdateSheetLength()
        {
            if (errorsContainer.HasErrors(() => Size) || sheetSizes == null)
            {
                Length = "0";
            }
            else
            {
                var val = sheetSizes[1] / 1000 * sheetSizes[2] / 1000;
                Length = val.ToString("0.000");
            }
        }

        private static double[] ParseSheetSize(string size)
        {
            if (size == null) return null;

            var parts = size.Split(new[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) return null;

            var sizes = parts[0].Split(new[] { '*' }, StringSplitOptions.RemoveEmptyEntries);
            if (sizes.Length != 3) return null;

            double d;
            if (sizes.All(x => double.TryParse(x, out d)))
            {
                return sizes.Select(double.Parse).ToArray();
            }

            return null;
        }
    }
}
