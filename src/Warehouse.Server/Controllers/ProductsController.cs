using System.Collections.Generic;
using System.Web.Http;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    public class ProductsController : ApiController
    {
        public IEnumerable<Product> Get()
        {
            return new []
            {
                new Product { Id = 123122 },
                new Product { Id = 245345 },
            };
        }
    }
}
