using EMStore.Services.ShoppingCartAPI.Dtos;

namespace EMStore.Services.ShoppingCartAPI.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDto> UpsertCartAsync(CartInputDto cartDto);
    }
}
