using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Warehouse.Api.Data;

namespace Warehouse.Api.Controllers
{
    public class TransactionsController : ApiController
    {
        private readonly IMongoContext context;

        public TransactionsController(IMongoContext context)
        {
            this.context = context;
        }

        public async Task<IHttpActionResult> Get()
        {
            var data = await context.Transactions.Find(new BsonDocument()).ToListAsync();
            return Ok(data);
        }
    }
}
