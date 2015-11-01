using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Warehouse.Api.Data;
using Warehouse.SharedModels;

namespace Warehouse.Api.Controllers
{
    public class OperationsController : ApiController
    {
        private readonly IMongoContext context;

        public OperationsController(IMongoContext context)
        {
            this.context = context;
        }

        // GET: api/Operations
        public async Task<IHttpActionResult> Get()
        {
            var data = await context.Operations.Find(new BsonDocument()).ToListAsync();
            return Ok(data);
        }

        // GET: api/Operations/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Operations
        public void Post([FromBody]CreateOperation value)
        {

        }

        // PUT: api/Operations/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Operations/5
        public void Delete(int id)
        {
        }
    }
}
