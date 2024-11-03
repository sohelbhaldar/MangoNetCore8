using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ITokenProvider
    {
        void ClearToken();
        string? GetToken();
        void SetToken(string token);
    }
}
