﻿
using EMStore.Services.OrdersAPI.Dtos;
using EMStore.Services.OrdersAPI.Services.IServices;
using Newtonsoft.Json;

namespace EMStore.Services.OrdersAPI.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
            }
            else
            {
                return [];
            }
        }
    }
}
