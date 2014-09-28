using System.Collections.Generic;
using System.Web.Http;
using Warehouse.Server.Data;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    public class ProductsController : ApiController
    {
        public IEnumerable<Product> Get()
        {
            var context = new MongoContext();
            var data = context.Products.FindAll();
            return data;
        }
    }
}
