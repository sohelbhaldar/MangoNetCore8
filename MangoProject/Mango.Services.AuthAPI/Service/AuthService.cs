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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public AuthService(RoleManager<IdentityRole> roleManager,
                            UserManager<ApplicationUser> userManager, AppDBContext dbContext, IJwtTokenGenerator jwtTokenGenerator)
        {

            _roleManager = roleManager;
            _userManager = userManager;
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var userDto = _dbContext.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (userDto != null)
            {
                if(roleName != null && !_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult()) 
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(userDto, roleName);    
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var userDto = _dbContext.ApplicationUsers.FirstOrDefault(u =>  u.UserName.ToLower() == loginRequestDTO.UserName.ToLower());

            var isValid = await _userManager.CheckPasswordAsync(userDto, loginRequestDTO.Password);

            if (!isValid | userDto == null)
            {
                return new LoginResponseDTO() { IdentityUser = null , Token ="" };
            }

            IdentityUserDTO identityUserDTO = new()
            {
                Email = userDto.Email,
                ID = userDto.Id,
                Name = userDto.Name,
                PhoneNumber = userDto.PhoneNumber
            };

            var userRoles = await _userManager.GetRolesAsync(userDto);

            var token =  _jwtTokenGenerator.GenerateToken(userDto, userRoles);
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                IdentityUser = identityUserDTO,
                Token = token
            };

            return loginResponseDTO;
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
                var userCreated = await _userManager.CreateAsync(user,registrationRequestDTO.Password);
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
