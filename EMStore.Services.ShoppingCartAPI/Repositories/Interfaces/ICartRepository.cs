using EMStore.Services.ShoppingCartAPI.Dtos;

namespace EMStore.Services.ShoppingCartAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto> UpsertCartAsync(CartInputDto cartDto);

        Task<bool> RemoveCartAsync(RemoveCartDto removeCartDto);

        Task<CartDto?> GetCartByUserIdAsync(string userId);

        Task<bool> ApplyCouponAsync(CartInputDto dto);

        Task<bool> RemoveCouponAsync(CartInputDto dto);

        Task EmailCartAsync(CartDto cartDto);
    }
}
