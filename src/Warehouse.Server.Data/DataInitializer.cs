using System.Data.Entity;
using Warehouse.Server.Models;

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
            db.Taxonomy.AddRange(new[]
            {
                new Taxonomy { Title = "Трубы", Sortorder = 0 },
                new Taxonomy { Title = "Листы", Sortorder = 1 },
                new Taxonomy { Title = "Квадрат", Sortorder = 2 },
                new Taxonomy { Title = "Арматура", Sortorder = 3 },
            });
        }
    }
}
