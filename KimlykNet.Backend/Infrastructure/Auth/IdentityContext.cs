using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class IdentityContext(DbContextOptions<IdentityContext> options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>(options);