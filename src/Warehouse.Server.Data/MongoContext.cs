using System;
using MongoDB.Driver;
using Warehouse.Server.Models;

namespace Warehouse.Server.Data
{
    public class MongoContext : IMongoContext, IDisposable
    {
        private MongoDatabase database;

        public MongoContext()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var server = client.GetServer();
            database = server.GetDatabase("skill");
        }

        public MongoDatabase Database { get { return database; } }

        public MongoCollection<Product> Products
        {
            get { return database.GetCollection<Product>("products"); }
        }

        #region Owin

        public static MongoContext Create()
        {
            return new MongoContext();
        }

        #endregion

        public void Dispose()
        {
        }
    }
}
