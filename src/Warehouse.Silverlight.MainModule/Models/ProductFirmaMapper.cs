using System.Collections.Generic;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.MainModule.Models
{
    public class ProductFirmaMapper : Dictionary<string, ProductFirma>
    {
        public ProductFirmaMapper()
        {
            Add(ProductFirmaNames.Skill, new ProductFirma { Translation = "Скилл" });
            Add(ProductFirmaNames.Fina, new ProductFirma { Translation = "Фина" });
            Add(ProductFirmaNames.Storage, new ProductFirma { Translation = "Хранение" });
            Add(ProductFirmaNames.Rozn, new ProductFirma { Translation = "Розница" });
        }
    }
}
