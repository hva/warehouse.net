using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Warehouse.Api.Data.Entities
{
    public class Transaction
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("date")]
        public DateTime DateTime { get; set; }

        [BsonElement("customer")]
        public string Customer { get; set; }

        [BsonElement("employee")]
        public string Employee { get; set; }

        [BsonElement("status")]
        public string Status { get; set; }

        [BsonElement("memos")]
        public List<Memo> Memos { get; set; }
    }
}
