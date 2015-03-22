using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using Warehouse.Server.Data;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    [Authorize]
    public class ProductsController : ApiController
    {
        private readonly IMongoContext context;

        public ProductsController(IMongoContext context)
        {
            this.context = context;
        }

        public IEnumerable<Product> Get()
        {
            return context.Products.FindAll();
        }

        public HttpResponseMessage Get(string id)
        {
            var data = context.Products.FindOneById(new ObjectId(id));
            if (data != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Put(string id, [FromBody] Product product)
        {
            var query = Query<Product>.EQ(p => p.Id, new ObjectId(id));
            var update = Update<Product>
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
            var res = context.Products.Update(query, update);
            var code = res.Ok ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            return Request.CreateResponse(code);
        }

        public HttpResponseMessage Post([FromBody] Product product)
        {
            var res = context.Products.Save(product);
            if (res.Ok)
            {
                return new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent(product.Id.ToString())
                };
            }
            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        public HttpResponseMessage Delete(string ids)
        {
            if (ids == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var arr = ids.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 0)
            {
                var objectIds = arr.Select(x => new ObjectId(x));
                var query = Query<Product>.In(x => x.Id, objectIds);
                var res = context.Products.Remove(query);
                if (res.Ok)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }

            return Request.CreateResponse(HttpStatusCode.InternalServerError);
        }

        [Route("api/products/updatePrice")]
        [HttpPut]
        public HttpResponseMessage UpdatePrice(ProductPriceUpdate[] items)
        {
            var bulk = context.Products.InitializeUnorderedBulkOperation();
            foreach (var x in items)
            {
                var query = Query<Product>.EQ(p => p.Id, new ObjectId(x.Id));
                var update = Update<Product>.Set(p => p.PriceOpt, x.NewPrice);
                bulk.Find(query).UpdateOne(update);
            }
            bulk.Execute();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/products/{id}/files")]
        [HttpPost]
        public HttpResponseMessage AttachFile(string id, FormDataCollection form)
        {
            var fileId = form.Get("fileId");
            var query = Query<Product>.EQ(p => p.Id, new ObjectId(id));
            var update = Update<Product>.AddToSet(p => p.Files, new ObjectId(fileId));
            var res = context.Products.Update(query, update);
            var code = res.Ok ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;
            return Request.CreateResponse(code);
        }

        [Route("api/products/{id}/files")]
        [HttpGet]
        public HttpResponseMessage GetFiles(string id)
        {
            var productId = new ObjectId(id);
            var product = context.Products.FindOneById(productId);
            var q = Query.In("_id", new BsonArray(product.Files));
            var files = context.Database.GridFS.Find(q);
            var data = files.Select(x => new FileInfo
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Size = x.Length,
                UploadDate = x.UploadDate,
            });
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}
