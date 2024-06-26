﻿using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models.Dtos.Cart
{
    public class CartHeaderDto
    {
        public int CartHeaderId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string CouponCode { get; set; } = string.Empty;


        public double Discount { get; set; }

 
        public double CartTotal { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }
}
