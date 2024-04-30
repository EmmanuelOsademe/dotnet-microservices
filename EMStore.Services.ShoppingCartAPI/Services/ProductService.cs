using EMStore.Services.ShoppingCartAPI.Dtos;
using EMStore.Services.ShoppingCartAPI.Services.IServices;
using EmStores.Services.ShoppingCartAPI.Dtos;
using Newtonsoft.Json;

namespace EMStore.Services.ShoppingCartAPI.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"/api/products");
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
