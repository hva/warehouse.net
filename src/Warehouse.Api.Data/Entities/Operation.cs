using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Warehouse.Api.Data.Entities
{
    public class Operation
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("customer")]
        public string Customer { get; set; }
    }
}
