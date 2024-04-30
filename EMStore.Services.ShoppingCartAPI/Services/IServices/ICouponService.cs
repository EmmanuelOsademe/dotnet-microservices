using EMStore.Services.ShoppingCartAPI.Dtos;

namespace EMStore.Services.ShoppingCartAPI.Services.IServices
{
    public interface ICouponService
    {
        Task<CouponDto> GetCouponAsync(string couponCode);
    }
}
