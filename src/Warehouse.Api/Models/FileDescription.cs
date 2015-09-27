using System;

namespace Warehouse.Api.Models
{
    public class FileDescription
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime UploadDate { get; set; }
        public FileMetadata Metadata { get; set; }
    }
}
