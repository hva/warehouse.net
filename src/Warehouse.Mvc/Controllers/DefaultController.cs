using System.Collections.Generic;
using System.Web.Http;
using Warehouse.Models;

namespace Warehouse.Mvc.Controllers
{
    public class DefaultController : ApiController
    {
        // GET api/values
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
