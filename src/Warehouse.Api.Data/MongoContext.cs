using MongoDB.Driver;
using Warehouse.Api.Data.Entities;

namespace Warehouse.Api.Data
{
    public class MongoContext : IMongoContext
    {
        public MongoContext(string connectionString)
        {
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            Database = client.GetDatabase(url.DatabaseName);

            Users = Database.GetCollection<User>(nameof(Users).ToLower());
            Products = Database.GetCollection<Product>(nameof(Products).ToLower());
            Operations = Database.GetCollection<Operation>(nameof(Operations).ToLower());
        }

        public IMongoDatabase Database { get; }
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<Operation> Operations { get; }
    }
}