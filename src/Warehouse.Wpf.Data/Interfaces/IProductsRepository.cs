using System.Collections.Generic;
using System.Threading.Tasks;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Data.Interfaces
{
    public interface IProductsRepository
    {
        Task<AsyncResult<Product[]>> GetAsync();
        Task<AsyncResult<Product>> GetAsync(string id);
        Task<AsyncResult<Product[]>> GetManyAsync(List<string> ids);
        //Task<AsyncResult<Product[]>> GetNamesAsync(List<string> ids);
        Task<AsyncResult<ProductName[]>> GetNamesAsync();
        Task<AsyncResult<string>> SaveAsync(Product product);
        Task<AsyncResult> UpdatePrice(ProductPriceUpdate[] prices);
        Task<AsyncResult> Delete(List<string> ids);
        Task<AsyncResult> AttachFile(string productId, string fileId);
        Task<AsyncResult<FileDescription[]>> GetFiles(string productId);
        Task<AsyncResult> DetachFiles(string productId, string[] fileIds);
    }
}
