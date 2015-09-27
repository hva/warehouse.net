using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Warehouse.Api.Models
{
    public class FileMetadata
    {
        [BsonElement("products")]
        public HashSet<ObjectId> ProductIds { get; set; }
    }
}
