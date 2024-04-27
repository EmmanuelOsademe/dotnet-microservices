using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
