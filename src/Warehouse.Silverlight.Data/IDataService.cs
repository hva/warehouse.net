using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.Data
{
    public interface IDataService
    {
        Task<AsyncResult<Product[]>> GetProductsAsync();
        Task<AsyncResult<Product>> GetProductAsync(string id);
        Task<AsyncResult> SaveProductAsync(Product product);
    }
}
