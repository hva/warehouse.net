namespace Warehouse.Server.Models
{
    public class Taxonomy
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public int Sortorder { get; set; }
    }
}
