using System.ComponentModel.DataAnnotations;

namespace EMStore.Services.OrdersAPI.Dtos
{
    public class OrderHeaderDto
    {
        public int OrderHeaderId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string CouponCode { get; set; } = string.Empty;

        [Required]
        public double Discount { get; set; }

        [Required]
        public double OrderTotal { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        public DateTime OrderTime { get; set; }

        public string Status { get; set; } = string.Empty;

        public string PaymentIntentId { get; set; } = string.Empty;

        public string StripeSessionId { get; set; } = string.Empty;
    }
}
