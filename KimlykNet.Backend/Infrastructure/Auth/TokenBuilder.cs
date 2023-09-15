using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using KimlykNet.Backend.Infrastructure.Configuration.Auth;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class TokenBuilder : ITokenBuilder
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthenticationOptions _authOptions;

    public TokenBuilder(UserManager<ApplicationUser> userManager, IOptions<AuthenticationOptions> authOptions)
    {
        _userManager = userManager;
        _authOptions = authOptions.Value;
    }

    public async Task<SecurityToken> CreateAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return null;
        }

        var access = await _userManager.CheckPasswordAsync(user, password);
        if (!access)
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_authOptions.SigningKey);
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, user.UserName),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName ?? string.Empty),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName ?? string.Empty),
            new(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_authOptions.Lifetime),
            Issuer = _authOptions.Issuer,
            Audience = _authOptions.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var tokenValue = tokenHandler.CreateToken(descriptor);
        var jwt = tokenHandler.WriteToken(tokenValue);
        return new SecurityToken
        {
            Token = jwt,
            Expiration = tokenValue.ValidTo
        };
    }
}
