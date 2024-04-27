using EMStore.Services.AuthAPI.DTOs;
using EMStore.Services.AuthAPI.Models;

namespace EMStore.Services.AuthAPI.Mappers
{
    public static class AuthMapper
    {

        public static UserDTO FromAppUserToUserDTO(this ApplicationUser applicationUser)
        {
            return new UserDTO
            {
                ID = applicationUser.Id,
                Name = applicationUser.Name,
                Email = applicationUser.Email??string.Empty,
                PhoneNumber = applicationUser.PhoneNumber ?? string.Empty
            };
        }
    }
}
