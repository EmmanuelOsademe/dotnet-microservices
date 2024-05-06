using EMStore.Services.EmailAPI.Dtos;
using EMStore.Services.EmailAPI.Dtos.Cart;

namespace EMStore.Services.EmailAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task EmailUserRegistrationAndLog(UserDTO userDto);
    }
}
