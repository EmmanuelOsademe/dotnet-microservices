﻿using EMStores.Web.Utility;
using System.ComponentModel.DataAnnotations;

namespace EMStores.Web.Models
{
	public class CreateProductDto
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		[Range(1, 1000)]
		public double Price { get; set; }

		[Required]
		public string Description { get; set; } = string.Empty;

		[Required]
		public string Category { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;

		[AllowedExtensions(new string[]{".jpg", ".png"})]
		[MaxFileSize(3)]
        public IFormFile? Image { get; set; }
    }
}
