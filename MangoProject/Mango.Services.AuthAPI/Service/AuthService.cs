using Mango.Service.AuthAPI.Data;
using Mango.Service.AuthAPI.Models;
using Mango.Service.AuthAPI.Models.DTO;
using Mango.Service.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Service.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDBContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthService(RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager, AppDBContext dbContext)
        {

            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;

        }
        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            ApplicationUser user = new ApplicationUser
            {
                Name = registrationRequestDTO.Name,
                Email = registrationRequestDTO.Email,
                UserName = registrationRequestDTO.Email,
                NormalizedUserName = registrationRequestDTO.Email.ToLower(),
                PhoneNumber = registrationRequestDTO.PhoneNumber
            };

            try
            {
                var userCreated = await _userManager.CreateAsync(user);
                if (userCreated != null && userCreated.Succeeded)
                {

                    var userToReturn = _dbContext.ApplicationUsers.Where(u => u.UserName == registrationRequestDTO.Email).FirstOrDefault();
                    IdentityUserDTO userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return userCreated?.Errors?.FirstOrDefault().Description;
                }
            }
            catch (Exception)
            {
            }
            return "Error Encountered";
        }
    }
}
