using System.Security.Claims;

namespace WepApi.Services
{
    public interface IUserService
    {
        Task<string?> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}