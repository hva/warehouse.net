using System.IO;
using System.Threading.Tasks;
using Warehouse.Wpf.Infrastructure;

namespace Warehouse.Wpf.Data.Interfaces
{
    public interface IFilesRepository
    {
        Task<AsyncResult<string>> Create(Stream stream, string fileName, string contentType);
    }
}
