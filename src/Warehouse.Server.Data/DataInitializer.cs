using System.Data.Entity;
using Warehouse.Models;

namespace Warehouse.Server.Data
{
    public class DataInitializer : CreateDatabaseIfNotExists<SklContext>
    {

        protected override void Seed(SklContext db)
        {
            AddTaxonomy(db);

            db.SaveChanges();
            base.Seed(db);
        }

        private static void AddTaxonomy(SklContext db)
        {
            db.Categories.AddRange(new[]
            {
                new Category { Title = "Taxonomy 1"},
                new Category { Title = "Taxonomy 2"},
                new Category { Title = "Taxonomy 3"},
            });
        }
    }
}
