﻿namespace EMStore.Services.OrdersAPI.Dtos
{
    public class OrderDetailsDto
    {
        public int OrderDetailsId { get; set; }

        public int OrderHeaderId { get; set; }

        public OrderHeaderDto? OrderHeader { get; set; }

        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
