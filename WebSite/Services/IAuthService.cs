using Shared.Models;

namespace WebSite.Services
{
    public interface IAuthService
    {
        Task Login(LoginModel loginRequest);     
        Task Logout();
    }
}
