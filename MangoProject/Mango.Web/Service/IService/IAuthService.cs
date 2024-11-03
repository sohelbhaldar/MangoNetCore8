using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO?> LoginAsync(LoginRequestDTO loginRequestDTO);
        Task<ResponseDTO?> RegisterAsync(RegistrationRequestDTO loginRequestDTO);
        Task<ResponseDTO?> AssignRoleAsync(RegistrationRequestDTO loginRequestDTO);
    }
}
