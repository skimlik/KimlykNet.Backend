using System.Security.Claims;

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
        string[] roles = { "SecurityAdministrators", "Administrators", "PendingUsers", "Users", "Family" };

        foreach (var role in roles)
        {
            await TryCreateRole(role);
        }

        var adminEmail = _configuration.GetValue<string>("Init:AdminEmail");
        if (string.IsNullOrEmpty(adminEmail))
        {
            throw new ApplicationException("Identity initialization configuration error. No email/username for the administartor configured");
        }

        var adminUserName = _configuration.GetValue<string>("Init:AdminUserName") ?? adminEmail;
        var admin = await TryCreateUserAsync(adminUserName, adminEmail, token);

        await TryAssignRoleAsync(admin, "SecurityAdministrators");
        await TryAssignRoleAsync(admin, "Administrators");
        await TryAssignRoleAsync(admin, "Users");

        string newPassword = _configuration.GetValue<string>("Init:Password");

        if (!string.IsNullOrWhiteSpace(newPassword))
        {
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(admin);
            await _userManager.ResetPasswordAsync(admin, resetToken, newPassword);
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

    private async Task TryAssignRoleAsync(ApplicationUser user, string roleName)
    {
        if (await _roleManager.RoleExistsAsync(roleName))
        {
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                _logger.LogWarning("User {User} has role {Role} already assigned. Skipping", user.NormalizedUserName, roleName);
                return;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    _logger.LogError("{Code} {Error}", error.Code, error.Description);
                }
            }

            return;
        }

        _logger.LogWarning("Role {Role} does not exist", roleName);
    }

    private async Task<ApplicationUser> TryCreateUserAsync(string user, string email, CancellationToken token)
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
                Gender = UserGender.Unknown,
                UserName = user,
            };

            var userResult = await _userManager.CreateAsync(newUser);
            LogError(userResult);
            return newUser;
        }

        return null;
    }

    private async Task<ApplicationRole> TryCreateRole(string role)
    {
        var roleExist = await _roleManager.RoleExistsAsync(role);
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
