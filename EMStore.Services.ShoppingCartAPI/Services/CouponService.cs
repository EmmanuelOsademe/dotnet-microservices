using EMStore.Services.ShoppingCartAPI.Dtos;
using EMStore.Services.ShoppingCartAPI.Services.IServices;
using Newtonsoft.Json;

namespace EMStore.Services.ShoppingCartAPI.Services
{
    public class CouponService(IHttpClientFactory clientFactory) : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory = clientFactory;
        public async Task<CouponDto> GetCouponAsync(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupons/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if (resp != null && resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
            }
            else
            {
                return new CouponDto();
            }
        }
    }
}
