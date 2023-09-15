using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class IdentityContext : IdentityDbContext<ApplicationUser>
{
    public IdentityContext(DbContextOptions<IdentityContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationRole> Roles { get; set; }

    public DbSet<ApplicationUser> Users { get; set; }
}