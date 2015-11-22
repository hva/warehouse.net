using MongoDB.Bson.Serialization.Attributes;

namespace Warehouse.Api.Data.Entities
{
    public class Memo
    {
        [BsonElement("price_opt")]
        public long PriceOpt { get; set; }

        [BsonElement("price_rozn")]
        public long PriceRozn { get; set; }

        [BsonElement("product")]
        public Product Product { get; set; }

    }
}
