using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Backend.Infrastructure.Configuration.Auth;
using KimlykNet.Contracts.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace KimlykNet.Backend.Controllers;

public class LoginController : ControllerBase
{
    private readonly ITokenBuilder _tokenBuilder;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthenticationOptions _authOptions;

    public LoginController(
        IOptions<AuthenticationOptions> authOptions,
        ITokenBuilder tokenBuilder,
        UserManager<ApplicationUser> userManager)
    {
        _tokenBuilder = tokenBuilder;
        _userManager = userManager;
        _authOptions = authOptions.Value;
    }

    [HttpPost]
    [Route("init")]
    public async Task<IActionResult> InitAsync(string secret)
    {
        var admin = _userManager.Users.SingleOrDefault(p => p.UserName == "superadmin");
        if (admin.PasswordHash is null)
        {
            var isPending = await _userManager.IsInRoleAsync(admin, "PendingUsers");
            if (isPending)
            {
                await _userManager.RemoveFromRoleAsync(admin, "PendingUsers");
                await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(admin);
                await _userManager.ResetPasswordAsync(admin, resetToken, secret);
            }
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    [Route("token")]
    public async Task<IActionResult> GenerateTokenAsync([FromBody] TokenGenerationRequest request)
    {
        var accessToken = await _tokenBuilder.CreateAsync(request.UserEmail, request.Password);
        if (accessToken?.Token == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        return Ok(accessToken);
    }
}