
namespace EMStore.Services.EmailAPI.Dtos.Cart
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }    
    }
}
