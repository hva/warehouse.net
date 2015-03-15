using System.IO;
using System.Threading.Tasks;
using Warehouse.Silverlight.Infrastructure;

namespace Warehouse.Silverlight.Data.Interfaces
{
    public interface IFilesRepository
    {
        Task<AsyncResult<string>> Create(Stream stream, string fileName, string contentType);
    }
}
