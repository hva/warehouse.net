using System.Collections.Generic;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
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

        public void Put(string id, [FromBody] Product product)
        {
            var context = new MongoContext();
            var query = Query<Product>.EQ(p => p.Id, new ObjectId(id));
            var update = Update<Product>.Set(p => p.Name, product.Name);
            var res = context.Products.Update(query, update);
        }
    }
}
