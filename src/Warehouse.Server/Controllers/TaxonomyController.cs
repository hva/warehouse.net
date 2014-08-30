using System.Collections.Generic;
using System.Web.Http;
using Warehouse.Models;

namespace Warehouse.Server.Controllers
{
    public class TaxonomyController : ApiController
    {
        public IEnumerable<Taxonomy> Get()
        {
            return new[]
            {
                new Taxonomy { Id = 1 },
                new Taxonomy { Id = 2 },
            };
        }
    }
}
