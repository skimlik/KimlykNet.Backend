using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class IdentityInitializer : BackgroundService, IInitializer
{
    private readonly IServiceProvider _services;
    private UserManager<ApplicationUser> _userManager;
    private RoleManager<ApplicationRole> _roleManager;
    private IConfiguration _configuration;
    private ILogger<IdentityInitializer> _logger;

    public IdentityInitializer(IServiceProvider services)
    {
        _services = services;
    }

    public async Task InitializeAsync(CancellationToken token)
    {
        var superAdminRole = await ShouldCreateRole("SuperAdmins", token);
        var adminRole = await ShouldCreateRole("Administrators", token);
        var usersRole = await ShouldCreateRole("Users", token);
        await ShouldCreateRole("PendingUsers", token);
        await ShouldCreateRole("Family", token);

        var admin = await ShouldCreateUser("superadmin", "superadmin@kimlyk.net", token);

        if (superAdminRole is not null && adminRole is not null && usersRole is not null && admin is not null)
        {
            await _userManager.AddToRoleAsync(admin, superAdminRole.Name);
            await _userManager.AddToRoleAsync(admin, adminRole.Name);
            await _userManager.AddToRoleAsync(admin, usersRole.Name);

            string newPassword = _configuration.GetValue<string>("Init:Password");

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(admin);
                await _userManager.ResetPasswordAsync(admin, resetToken, newPassword);
            }
        }
        else
        {
            _logger.LogError("Identity initialization failed, see previous errors");
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("IdentityInitializer stopped. Initialization finished");
        await base.StopAsync(stoppingToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _services.CreateScope();

            _userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            _configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            _logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<IdentityInitializer>();
            _logger.LogInformation("IdentityInitializer service started");

            await InitializeAsync(stoppingToken);
        }
    }

    private async Task<ApplicationUser> ShouldCreateUser(string user, string email, CancellationToken token)
    {
        var userExist = await _userManager.Users.AnyAsync(p => p.UserName == user, token);

        if (!userExist)
        {
            _logger.LogInformation("'{User}' user does not exist, seeding.", user);
            var normalizedEmail = _userManager.NormalizeEmail(email);
            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                EmailConfirmed = true,
                FamilyMember = true,
                LockoutEnabled = false,
                NormalizedEmail = normalizedEmail,
                NormalizedUserName = normalizedEmail,
                TwoFactorEnabled = false,
                UserName = user,
            };

            var userResult = await _userManager.CreateAsync(newUser);
            LogError(userResult);
            return newUser;
        }

        return null;
    }

    private async Task<ApplicationRole> ShouldCreateRole(string role, CancellationToken token)
    {
        var roleExist = await _roleManager.Roles.AnyAsync(p => p.Name == role, token);
        if (!roleExist)
        {
            _logger.LogInformation("'{Role}' role does not exist, seeding.", role);

            ApplicationRole newRole = new ApplicationRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = role,
                NormalizedName = role,
            };

            var roleResult = await _roleManager.CreateAsync(newRole);

            LogError(roleResult);
            return newRole;
        }
        return null;
    }

    private void LogError(IdentityResult result)
    {
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _logger.LogError("{Code}: {Description}", error.Code, error.Description);
            }
        }
    }
}
