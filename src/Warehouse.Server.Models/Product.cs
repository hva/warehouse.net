using System.Runtime.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Warehouse.Server.Models
{
    public class Product
    {
        [BsonId]
        [IgnoreDataMember]
        public ObjectId MongoId { get; set; }

        public string Id
        {
            get { return MongoId.ToString(); }
            set { MongoId = ObjectId.Parse(value); }
        }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("size")]
        public string Size { get; set; }

        [BsonElement("k")]
        public double K { get; set; }

        [BsonElement("price_opt")]
        public int PriceOpt { get; set; }

        [BsonElement("price_rozn")]
        public int PriceRozn { get; set; }

        [BsonElement("weight")]
        public double Weight { get; set; }

        [BsonElement("count")]
        public int Count { get; set; }

        [BsonElement("nd")]
        public double[] Nd { get; set; }

        [BsonElement("length")]
        public double Length { get; set; }

        [BsonElement("1C")]
        public object Link1C { get; set; }

        [BsonElement("price_income")]
        public long PriceIcome { get; set; }
    }
}
