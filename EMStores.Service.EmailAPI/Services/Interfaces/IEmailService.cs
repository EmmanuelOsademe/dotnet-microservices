using EMStore.Services.EmailAPI.Dtos;
using EMStore.Services.EmailAPI.Dtos.Cart;
using EMStore.Services.EmailAPI.Message;

namespace EMStore.Services.EmailAPI.Services.Interfaces
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task EmailUserRegistrationAndLog(UserDTO userDto);

        Task LogAndEmailPlacedOrder(RewardMessage rewardMessage);
    }
}
