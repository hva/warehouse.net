using System.ComponentModel.DataAnnotations;

namespace Warehouse.Api.Data.Models
{
    public class CreateUserModel : UpdateUserModel
    {
        [Required]
        public string Password { get; set; }
    }
}