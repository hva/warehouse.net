using System;

namespace Warehouse.Silverlight.Models
{
    public class FileDescription
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
