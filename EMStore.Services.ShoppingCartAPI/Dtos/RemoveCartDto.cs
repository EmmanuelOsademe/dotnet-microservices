using System.ComponentModel.DataAnnotations;

namespace EMStore.Services.ShoppingCartAPI.Dtos
{
    public class RemoveCartDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public int CartDetailsId { get; set; }  
    }
}
