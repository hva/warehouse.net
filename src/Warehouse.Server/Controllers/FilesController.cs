using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
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
                var serverFileName = fileData.LocalFileName;
                var clientFileName = fileData.Headers.ContentDisposition.FileName;

                var fileId = Upload(serverFileName, clientFileName);

                if (AddFileToProduct(fileId, productId))
                {
                    File.Delete(serverFileName);
                    return Request.CreateResponse(HttpStatusCode.Created);
                }
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        private string Upload(string serverFileName, string clientFileName)
        {
            using (var fs = new FileStream(serverFileName, FileMode.Open))
            {
                var info = context.Database.GridFS.Upload(fs, clientFileName);
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
