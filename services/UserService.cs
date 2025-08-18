using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WepApi.Areas.Identity.Data;

namespace WepApi.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    public UserService(UserManager<User> userManager) => _userManager = userManager;

    public async Task<String?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        var userId = user.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
        if (string.IsNullOrEmpty(userId)) return null;

        var userObj = await _userManager.FindByIdAsync(userId);
        if (userObj == null) return null;
        return userObj.GetFullName();
    }
}
    