using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task<AsyncResult> UpdatePrice(ProductPriceUpdate[] prices);
        Task<AsyncResult> Delete(List<string> ids);
    }
}
