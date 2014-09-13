using System.Data.Entity;
using Warehouse.Server.Models;

namespace Warehouse.Server.Data
{
    public class SklContext : DbContext
    {
        public SklContext()
        {
            Database.SetInitializer(new DataInitializer());
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Taxonomy> Taxonomy { get; set; }
    }
}
