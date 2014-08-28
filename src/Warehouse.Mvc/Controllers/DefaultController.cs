using System.Collections.Generic;
using System.Web.Http;

namespace Warehouse.Mvc.Controllers
{
    public class DefaultController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}
