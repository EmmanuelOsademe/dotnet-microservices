﻿using EMStore.Services.OrdersAPI.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMStore.Services.OrdersAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }

        public int OrderHeaderId { get; set; }

        [ForeignKey("OrderHeaderId")]
        public OrderHeader? OrderHeader { get; set; }

        public int ProductId { get; set; }

        [NotMapped]
        public ProductDto? Product { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public double Price { get; set; }
    }
}
