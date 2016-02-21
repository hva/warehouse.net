using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Warehouse.Api.Data;
using Warehouse.Api.Data.Entities;
using Warehouse.Api.Models;

namespace Warehouse.Api.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        private readonly IMongoContext context;

        public ProductsController(IMongoContext context)
        {
            this.context = context;
        }

        public Task<List<Product>> Get()
        {
            return context.Products.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<IHttpActionResult> Get(string id)
        {
            var filter = Builders<Product>.Filter.Eq(x => x.Id, new ObjectId(id));
            var data = await context.Products.Find(filter).FirstOrDefaultAsync();
            if (data != null)
            {
                return Ok(data);
            }
            return NotFound();
        }

        [Route("api/products/getMany")]
        [HttpPost]
        public async Task<IHttpActionResult> GetMany(string[] ids)
        {
            if (ids == null)
            {
                return BadRequest("invalid ids");
            }

            var objectIds = ids.Select(x => new ObjectId(x)).ToArray();

            var filter = Builders<Product>.Filter.In(x => x.Id, objectIds);
            var data = await context.Products.Find(filter).ToListAsync();
            return Ok(data);
        }

        [Route("api/products/getNames")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNames()
        {
            var data = await context.Products
                .Find(new BsonDocument())
                .Project(x => new { x.Id, Name = string.Concat(x.Name, " ", x.Size) })
                .ToListAsync();

            return Ok(data);
        }

        public async Task<IHttpActionResult> Put(string id, [FromBody] Product product)
        {
            var query = Builders<Product>.Filter.Eq(p => p.Id, new ObjectId(id));
            var update = Builders<Product>.Update
                .Set(p => p.Name, product.Name)
                .Set(p => p.Size, product.Size)
                .Set(p => p.K, product.K)
                .Set(p => p.PriceOpt, product.PriceOpt)
                .Set(p => p.PriceRozn, product.PriceRozn)
                .Set(p => p.Weight, product.Weight)
                .Set(p => p.Count, product.Count)
                .Set(p => p.Nd, product.Nd)
                .Set(p => p.Length, product.Length)
                .Set(p => p.PriceIcome, product.PriceIcome)
                .Set(p => p.Internal, product.Internal)
                .Set(p => p.Firma, product.Firma)
            ;
            var res = await context.Products.UpdateOneAsync(query, update);
            if (res != null)
            {
                return Ok();
            }

            return InternalServerError();
        }

        public async Task<IHttpActionResult> Post([FromBody] Product product)
        {
            await context.Products.InsertOneAsync(product);
            return Created(string.Empty, product.Id);
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
                var query = Builders<Product>.Filter.In(x => x.Id, objectIds);
                var res = await context.Products.DeleteManyAsync(query);
                if (res != null)
                {
                    return Ok();
                }
            }

            return InternalServerError();
        }

        [Route("api/products/updatePrice")]
        [HttpPut]
        public async Task<IHttpActionResult> UpdatePrice(ProductPriceUpdate[] items)
        {
            var updates = items.Select(x =>
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, new ObjectId(x.Id));
                var update = Builders<Product>.Update
                    .Set(p => p.PriceOpt, x.NewPriceOpt)
                    .Set(p => p.PriceRozn, x.NewPriceRozn);
                return new UpdateOneModel<Product>(filter, update);
            });

            var res = await context.Products.BulkWriteAsync(updates);
            if (res != null)
            {
                return Ok();
            }

            return InternalServerError();
        }
    }
}
