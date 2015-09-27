using System.ComponentModel.DataAnnotations;

namespace Warehouse.Api.Models
{
    public class ChangePassword
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}