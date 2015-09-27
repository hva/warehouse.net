using System;
using System.Collections.Generic;
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
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Warehouse.Api.Interfaces;
using Warehouse.Api.Models;

namespace Warehouse.Api.Controllers
{
    [Authorize]
    public class FilesController : ApiController
    {
        private readonly IMongoContext context;

        public FilesController(IMongoContext context)
        {
            this.context = context;
        }

        public async Task<IHttpActionResult> Get()
        {
            var bucket = new GridFSBucket(context.Database);
            var files = await bucket.FindAsync(new BsonDocument());
            var data = await files.ToListAsync();
            var res = data.Select(x => new FileDescription
            {
                Id = x.Id.ToString(),
                Name = x.Filename,
                Size = x.Length,
                UploadDate = x.UploadDateTime,
                Metadata = (x.Metadata != null) ? BsonSerializer.Deserialize<FileMetadata>(x.Metadata) : null
            });

            return Ok(res);
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string id)
        {
            var bucket = new GridFSBucket(context.Database);
            // TODO: use stream instead of bytes
            var bytes = await bucket.DownloadAsBytesAsync(new ObjectId(id), new GridFSDownloadOptions());
            var resp = Request.CreateResponse();
            resp.Content = new ByteArrayContent(bytes);
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Jpeg);
            return ResponseMessage(resp);
        }

        public async Task<IHttpActionResult> Post()
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

                // TODO: do we need content type?
                // var contentType = fileData.Headers.ContentDisposition.Name;

                var fileId = await UploadAsync(file, remoteFileName);

                File.Delete(file);

                return Created(string.Empty, fileId);
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var arr = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (arr.Length > 0)
            {
                var bucket = new GridFSBucket(context.Database);
                var tasks = arr.Select(x => bucket.DeleteAsync(new ObjectId(x))).ToArray();
                await Task.WhenAll(tasks);
            }
            return Ok();
        }

        [Route("api/files/{id}/products")]
        [HttpPost]
        public async Task<IHttpActionResult> AttachProducts(string id, string[] productIds)
        {
            if (productIds == null)
            {
                return BadRequest();
            }

            var bucket = new GridFSBucket(context.Database);
            var objectId = new ObjectId(id);

            // TODO: find a way to update metadata

            // loading file info
            string filename;
            using (var stream = await bucket.OpenDownloadStreamAsync(objectId))
            {
                filename = stream.FileInfo.Filename;
                await stream.CloseAsync();
            }

            // loading file content
            // TODO: use stream instead of bytes
            var bytes = await bucket.DownloadAsBytesAsync(objectId, new GridFSDownloadOptions());

            // save updated file duplicate
            var ids = productIds.Select(x => new ObjectId(x));
            var meta = new FileMetadata { ProductIds = new HashSet<ObjectId>(ids) };
            var options =  new GridFSUploadOptions { Metadata = meta.ToBsonDocument() };
            var createdObjectId = await bucket.UploadFromBytesAsync(filename, bytes, options);

            // removing original file
            await bucket.DeleteAsync(objectId);

            return Created(string.Empty, createdObjectId.ToString());
        }

        private async Task<string> UploadAsync(string file, string remoteFileName)
        {
            var bucket = new GridFSBucket(context.Database);
            using (var fs = new FileStream(file, FileMode.Open))
            {
                var objectId = await bucket.UploadFromStreamAsync(remoteFileName, fs);
                return objectId.ToString();
            }
        }
    }
}
