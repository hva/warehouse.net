using MongoDB.Driver;
using Warehouse.Server.Models;

namespace Warehouse.Server.Data
{
    public class MongoContext
    {
        private MongoDatabase database;

        public MongoContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();
            database = server.GetDatabase("skill");
        }

        public MongoCollection<Product> Products
        {
            get { return database.GetCollection<Product>("products"); }
        }
    }
}
