using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;
using EMStores.Web.Models.Dtos.Cart;

namespace EMStores.Web.Services.IServices
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartInputDto inputDto);
        Task<ResponseDto?> RemoveFromCartAsync (RemoveCartDto inputDto);
        Task<ResponseDto?> ApplyCouponAsync(CartInputDto inputDto);
        Task<ResponseDto?> RemoveCouponAsync(CartInputDto inputDto);

        Task<ResponseDto?> EmailCartAsync(CartDto cartDto);
    }
}
