namespace Mango.Service.AuthAPI.Models.DTO
{
    public class LoginResponseDTO
    {
        public IdentityUserDTO IdentityUser { get; set; }
        public string Token { get; set; }
    }
}
