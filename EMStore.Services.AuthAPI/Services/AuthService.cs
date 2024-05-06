using EMStore.Services.AuthAPI.Data;
using EMStore.Services.AuthAPI.DTOs;
using EMStore.Services.AuthAPI.Mappers;
using EMStore.Services.AuthAPI.Models;
using EMStore.Services.AuthAPI.Services.IServices;
using EMStores.MessageBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMStore.Services.AuthAPI.Services
{
    public class AuthService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator, IMessageBus messageBus, IConfiguration config) : IAuthService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IConfiguration _config = config;

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null) return false;

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
               await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return true;
        }

        public async Task<LoginResponseDTO?> Login(LoginRequestDTO requestDTO)
        {
            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == requestDTO.UserName.ToLower());

            if (user == null)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }

            bool isValid = await _userManager.CheckPasswordAsync(user, requestDTO.Password);

            if (!isValid)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // User is found and password is valid, so generate JWT
            string token = _jwtTokenGenerator.GenerateToken(user, roles);

            UserDTO userDTO = user.FromAppUserToUserDTO();
            LoginResponseDTO loginResponseDTO = new()
            {
                User = userDTO,
                Token = token
            };
            return loginResponseDTO;
        }

        public async Task<string> Register(RegistrationRequestDTO requestDTO)
        {
            ApplicationUser user = new()
            {
                UserName = requestDTO.Email,
                Email = requestDTO.Email,
                NormalizedEmail = requestDTO.Email,
                Name = requestDTO.Name,
                PhoneNumber = requestDTO.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, requestDTO.Password);
                if (result.Succeeded)
                {
                    var returnedUser = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == requestDTO.Email);
                    if (returnedUser != null)
                    {
                        UserDTO userDto = returnedUser.FromAppUserToUserDTO();
                        await EmailUserRegistrationAsync(userDto);
                        return "";
                    }
                    else
                    {
                        return "User not found";
                    }
                    
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private async Task EmailUserRegistrationAsync(UserDTO userDto)
        {
            string serviceBusConnectionString = _config.GetValue<string>("ServiceBusConnectionString") ?? string.Empty;
            string topicQueueName = _config.GetValue<string>("TopicAndQueueNames:EmailUserRegistrationQueue") ?? string.Empty;
            await _messageBus.PublishMessage(userDto, topicQueueName, serviceBusConnectionString);

        }
    }
}
