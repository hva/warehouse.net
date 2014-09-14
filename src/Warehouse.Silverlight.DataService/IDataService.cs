using System.Threading.Tasks;
using Warehouse.Silverlight.Models;

namespace Warehouse.Silverlight.DataService
{
    public interface IDataService
    {
        Task<Taxonomy[]> GetTaxonomyAsync();
    }
}
