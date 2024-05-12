namespace EMStores.Web.Models
{
    public class OrderDto
    {
        public OrderHeaderDto? OrderHeader { get; set; }

        public IEnumerable<OrderDetailsDto>? OrderDetails { get; set; }
    }
}
