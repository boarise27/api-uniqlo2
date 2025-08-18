using Microsoft.AspNetCore.Identity;

namespace WepApi.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; } = false;


    public string GetFullName()
    {
        return $"{FirstName} {LastName}".Trim();
    }
}   

