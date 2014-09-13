using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Warehouse.Server.Data;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    public class TaxonomyController : ApiController
    {
        public IEnumerable<Taxonomy> Get()
        {
            var context = new SklContext();
            return context.Taxonomy.ToArray();
        }
    }
}
