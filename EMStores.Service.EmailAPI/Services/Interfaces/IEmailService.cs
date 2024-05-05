using EMStore.Services.EmailAPI.Dtos.Cart;

namespace EMStore.Services.EmailAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
    }
}
