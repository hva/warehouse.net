using System.Collections.Generic;

namespace Warehouse.Wpf.Models
{
    public class ProductName
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductNameComparer : IEqualityComparer<ProductName>
    {
        public bool Equals(ProductName x, ProductName y)
        {
            return string.Equals(x.Id, y.Id);
        }

        public int GetHashCode(ProductName x)
        {
            return x.Id.GetHashCode();
        }
    }
}
