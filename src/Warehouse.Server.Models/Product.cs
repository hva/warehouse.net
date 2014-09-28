using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Warehouse.Server.Models
{
    public class Product
    {
        [BsonId]
        [JsonIgnore]
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

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [BsonElement("weight")]
        public double Weight { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [BsonElement("count")]
        public int Count { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [BsonElement("nd")]
        public double[] Nd { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [BsonElement("length")]
        public double Length { get; set; }

        [BsonElement("1C")]
        public string Internal { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        [BsonElement("price_income")]
        public long PriceIcome { get; set; }
    }
}
