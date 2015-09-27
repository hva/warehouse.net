using System.ComponentModel.DataAnnotations;

namespace Warehouse.Api.Models
{
    public class UpdateUserModel
    {
        [Required]
        public string UserName { get; set; }

        public string[] Roles { get; set; }
    }
}