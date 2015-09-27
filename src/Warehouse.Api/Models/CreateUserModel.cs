using System.ComponentModel.DataAnnotations;

namespace Warehouse.Api.Models
{
    public class CreateUserModel : UpdateUserModel
    {
        [Required]
        public string Password { get; set; }
    }
}