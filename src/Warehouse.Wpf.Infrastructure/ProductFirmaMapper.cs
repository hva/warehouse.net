using System.Collections.Generic;

namespace Warehouse.Wpf.Infrastructure
{
    public class ProductFirmaMapper : Dictionary<string, ProductFirma>
    {
        public ProductFirmaMapper()
        {
            Add(ProductFirmaNames.Skill, new ProductFirma { Translation = "Скилл", Color = "#00FF00"});
            Add(ProductFirmaNames.Fina, new ProductFirma { Translation = "Фина", Color = "#0000FF" });
            Add(ProductFirmaNames.Storage, new ProductFirma { Translation = "Хранение", Color = "#9400D3" });
            Add(ProductFirmaNames.Rozn, new ProductFirma { Translation = "Розница", Color = "#FF0000" });
        }
    }
}
