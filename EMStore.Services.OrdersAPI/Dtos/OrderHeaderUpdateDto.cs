namespace EMStore.Services.OrdersAPI.Dtos
{
    public class OrderHeaderUpdateDto
    {
        public int OrderHeaderId { get; set; }

        public string StripeSessionId { get; set; } = string.Empty;

        public string PaymentIntentId { get; set; } = string.Empty;

        public string Status { get; set;} = string.Empty;
    }
}
