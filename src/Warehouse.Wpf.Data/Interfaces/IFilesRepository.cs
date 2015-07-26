using System.IO;
using System.Threading.Tasks;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Data.Interfaces
{
    public interface IFilesRepository
    {
        Task<AsyncResult<FileDescription[]>> GetAll();
        Task<AsyncResult> DeleteAsync(string[] fileIds);
        Task<AsyncResult<string>> Create(Stream stream, string fileName, string contentType);
        Task<AsyncResult> AttachProducts(string fileId, string[] productIds);
    }
}
