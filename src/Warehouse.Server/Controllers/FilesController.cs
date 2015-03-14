using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using Warehouse.Server.Data;
using Warehouse.Server.Models;

namespace Warehouse.Server.Controllers
{
    [Authorize]
    public class FilesController : ApiController
    {
        private readonly IMongoContext context;

        public FilesController(IMongoContext context)
        {
            this.context = context;
        }

        public HttpResponseMessage Get(string id)
        {
            ObjectId fileId;
            if (!ObjectId.TryParse(id, out fileId))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var file = context.Database.GridFS.FindOneById(fileId);
            if (file == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var stream = file.OpenRead();
            var resp = Request.CreateResponse();
            resp.Content = new StreamContent(stream);
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Jpeg);
            return resp;
        }

        public async Task<HttpResponseMessage> Post(string productId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var root = HttpContext.Current.Server.MapPath("~/App_Data/uploads");

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }

            var provider = new MultipartFormDataStreamProvider(root);

            await Request.Content.ReadAsMultipartAsync(provider);
            var fileData = provider.FileData.FirstOrDefault();

            if (fileData != null)
            {
                var file = fileData.LocalFileName;
                var remoteFileName = fileData.Headers.ContentDisposition.FileName;
                var contentType = fileData.Headers.ContentDisposition.Name;

                var fileId = Upload(file, remoteFileName, contentType);

                if (AddFileToProduct(fileId, productId))
                {
                    File.Delete(file);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        private string Upload(string file, string remoteFileName, string contentType)
        {
            using (var fs = new FileStream(file, FileMode.Open))
            {
                var options = new MongoGridFSCreateOptions { ContentType = contentType };
                var info = context.Database.GridFS.Upload(fs, remoteFileName, options);
                return info.Id.ToString();
            }
        }

        private bool AddFileToProduct(string fileId, string productId)
        {
            var query = Query<Product>.EQ(p => p.Id, new ObjectId(productId));
            var update = Update<Product>.AddToSet(p => p.Files, new ObjectId(fileId));
            var res = context.Products.Update(query, update);
            return res.Ok;
        }
    }
}
