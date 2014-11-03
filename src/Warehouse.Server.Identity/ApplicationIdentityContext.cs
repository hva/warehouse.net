﻿using System;
using AspNet.Identity.MongoDB;
using MongoDB.Driver;

namespace Warehouse.Server.Identity
{
    public class ApplicationIdentityContext : IdentityContext, IDisposable
    {
        public ApplicationIdentityContext(MongoCollection users) : base(users)
        {
        }

        public static ApplicationIdentityContext Create()
        {
            // todo add settings where appropriate to switch server & database in your own application
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetServer().GetDatabase("skill");
            var users = database.GetCollection<IdentityUser>("users");
            //var roles = database.GetCollection<IdentityRole>("roles");
            return new ApplicationIdentityContext(users/*, roles*/);
        }

        public void Dispose()
        {
        }
    }
}