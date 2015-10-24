using MongoDB.Driver;
using Warehouse.Api.Data.Entities;

namespace Warehouse.Api.Data
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<Product> Products { get; }
    }
}