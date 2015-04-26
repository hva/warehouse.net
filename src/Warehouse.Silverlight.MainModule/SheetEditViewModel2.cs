using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.MainModule
{
    public class SheetEditViewModel2 : ProductEditViewModel2
    {
        public SheetEditViewModel2(Product product, bool canEditPrice) : base(product, canEditPrice)
        {
        }

        public override string LenghtLabel { get { return "Площадь листа (м²)"; } }

        public override string NdLabel  { get { return "Н/Д (м²)"; } }

    }
}
