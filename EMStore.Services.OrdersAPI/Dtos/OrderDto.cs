namespace EMStore.Services.OrdersAPI.Dtos
{
    public class OrderDto
    {
        public OrderHeaderDto? OrderHeader { get; set; }

        public  IEnumerable<OrderDetailsDto>? OrderDetails { get; set; }
    }
}
