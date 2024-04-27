using EMStore.Services.AuthAPI.DTOs;

namespace EMStore.Services.AuthAPI.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDTO requestDTO);

        Task<LoginResponseDTO?> Login(LoginRequestDTO requestDTO);

        Task<bool> AssignRole(string email, string roleName);
    }
}
