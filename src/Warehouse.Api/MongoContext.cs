using System.Configuration;
using MongoDB.Driver;
using Warehouse.Api.Entities;
using Warehouse.Api.Interfaces;

namespace Warehouse.Api
{
    public class MongoContext : IMongoContext
    {
        public MongoContext()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["MongoDB"].ConnectionString;
            var url = new MongoUrl(connectionString);
            var client = new MongoClient(url);
            Database = client.GetDatabase(url.DatabaseName);

            Users = Database.GetCollection<User>(nameof(Users).ToLower());
            Products = Database.GetCollection<Product>(nameof(Products).ToLower());
        }

        public IMongoDatabase Database { get; }
        public IMongoCollection<User> Users { get; }
        public IMongoCollection<Product> Products { get; }
    }
}