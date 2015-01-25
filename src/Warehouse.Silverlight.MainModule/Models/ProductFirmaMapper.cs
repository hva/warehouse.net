using System.Collections.Generic;
using System.Windows.Media;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.MainModule.Models
{
    public class ProductFirmaMapper : Dictionary<string, ProductFirma>
    {
        public ProductFirmaMapper()
        {
            Add(ProductFirmaNames.Skill, new ProductFirma { Translation = "Скилл", Color = Color.FromArgb(255, 0, 255, 0) });
            Add(ProductFirmaNames.Fina, new ProductFirma { Translation = "Фина", Color = Color.FromArgb(255, 0, 0, 255) });
            Add(ProductFirmaNames.Storage, new ProductFirma { Translation = "Хранение", Color = Color.FromArgb(255, 148, 0, 211) });
            Add(ProductFirmaNames.Rozn, new ProductFirma { Translation = "Розница", Color = Color.FromArgb(255, 255, 0, 0) });
        }
    }
}
