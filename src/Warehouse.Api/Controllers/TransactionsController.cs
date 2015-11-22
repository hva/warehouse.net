using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Warehouse.Api.Data;
using Warehouse.Api.Data.Entities;

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

        public async Task<IHttpActionResult> Post(Transaction model)
        {
            await context.Transactions.InsertOneAsync(model);
            return Created(string.Empty, model.Id.ToString());
        }

        public async Task<IHttpActionResult> Put(string id, Transaction transaction)
        {
            var query = Builders<Transaction>.Filter.Eq(p => p.Id, new ObjectId(id));
            var update = Builders<Transaction>.Update
                .Set(p => p.Customer, transaction.Customer)
                .Set(p => p.Memos, transaction.Memos)
                .Set(p => p.DateTime, transaction.DateTime)
                .Set(p => p.Status, transaction.Status)
            ;
            var res = await context.Transactions.UpdateOneAsync(query, update);
            if (res != null)
            {
                return Ok();
            }

            return InternalServerError();
        }

        public async Task<IHttpActionResult> Delete(string ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var arr = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 0)
            {
                var objectIds = arr.Select(x => new ObjectId(x));
                var query = Builders<Transaction>.Filter.In(x => x.Id, objectIds);
                var res = await context.Transactions.DeleteManyAsync(query);
                if (res != null)
                {
                    return Ok();
                }
            }

            return InternalServerError();
        }
    }
}
