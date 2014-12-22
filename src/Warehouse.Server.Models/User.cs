using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace Warehouse.Server.Models
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonElement("UserName")]
        public string UserName { get; set; }

        [JsonIgnore]
        [BsonElement("Roles")]
        public string[] Roles { get; set; }

        public string Role
        {
            get
            {
                if (Roles != null && Roles.Length > 0)
                {
                    return Roles[0];
                }
                return null;
            }
        }
    }
}
