using MongoDB.Driver;
using Warehouse.Api.Entities;

namespace Warehouse.Api.Interfaces
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
        IMongoCollection<User> Users { get; }
        IMongoCollection<Product> Products { get; }
    }
}