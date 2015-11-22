using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Warehouse.Wpf.Infrastructure
{
    public static class CollectionExtensions
    {
        public static Collection<T> AddRange2<T>(this Collection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new System.ArgumentNullException("collection");
            if (items == null) throw new System.ArgumentNullException("items");

            foreach (var each in items)
            {
                collection.Add(each);
            }

            return collection;
        }
    }
}
