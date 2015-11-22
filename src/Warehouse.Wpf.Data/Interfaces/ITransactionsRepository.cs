using System.Threading.Tasks;
using Warehouse.Wpf.Infrastructure;
using Warehouse.Wpf.Models;

namespace Warehouse.Wpf.Data.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<AsyncResult<TransactionModel[]>> GetAsync();
        Task<AsyncResult<string>> SaveAsync(TransactionModel transaction);
        Task<AsyncResult> DeleteAsync(string[] ids);
    }
}
