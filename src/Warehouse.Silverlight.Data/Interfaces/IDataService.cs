using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.Data.Interfaces
{
    public interface IDataService
    {
        Task<AsyncResult<Product>> GetProductAsync(string id);
        Task<AsyncResult<string>> SaveProductAsync(Product product);
    }
}
