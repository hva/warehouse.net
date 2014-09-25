using System.Collections.Generic;
using System.Web.Http;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    public class TaxonomyController : ApiController
    {
        public IEnumerable<Taxonomy> Get()
        {
            return new Taxonomy[] {};
        }
    }
}
