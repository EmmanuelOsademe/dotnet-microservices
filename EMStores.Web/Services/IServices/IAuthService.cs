using EMStores.Web.Models;
using EMStores.Web.Models.Dtos;

namespace EMStores.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<ResponseDto?> RegisterAsync(RegistrationRequestDTO registrationRequestDTO);

        Task<ResponseDto?> AssignRoleAsync (RegistrationRequestDTO registrationRequestDTO);
    }
}
