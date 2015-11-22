﻿using System;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Warehouse.Api.Data.Entities.Converters
{
    public class ObjectIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var token = JToken.Load(reader);
            return new ObjectId(token.ToObject<string>());
        }

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(ObjectId));
        }
    }
}
