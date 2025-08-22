using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WepApi.Areas.Identity.Data;

namespace WepApi.Areas.Identity.Data;

public class WepApiIdentityDbContext : IdentityDbContext<User>
{
    

    public WepApiIdentityDbContext(DbContextOptions<WepApiIdentityDbContext> options)
        : base(options)
    {
    }

    public override DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
