using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Warehouse.Models;
using Warehouse.Server.Data;

namespace Warehouse.Server.Controllers
{
    public class TaxonomyController : ApiController
    {
        public IEnumerable<Category> Get()
        {
            var context = new SklContext();
            return context.Categories.ToArray();
        }
    }
}
