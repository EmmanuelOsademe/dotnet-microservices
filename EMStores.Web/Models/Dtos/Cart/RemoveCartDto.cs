using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models.Dtos.Cart
{
    public class RemoveCartDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int CartDetailsId { get; set; }  
    }
}
