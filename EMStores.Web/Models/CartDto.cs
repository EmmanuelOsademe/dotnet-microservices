using EMStores.Web.Models.Dtos.Cart;

namespace EMStores.Web.Models
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }    
    }
}
