using System.Security.Claims;

using Microsoft.IdentityModel.JsonWebTokens;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class UserContextAccessor : IUserContextAccessor
{
    private const StringComparison ordinalIgnoreCase = StringComparison.OrdinalIgnoreCase;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public UserInfo GetUserInfo()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var identity = user.Identity;

        if (!IsAuthenticated)
        {
            return null;
        }

        string firstName = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GivenName, ordinalIgnoreCase))?.Value;
        string lastName = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname, ordinalIgnoreCase))?.Value;

        return new UserInfo
        {
            Name = identity.Name,
            Email = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, ordinalIgnoreCase))?.Value,
            FirstName = string.IsNullOrWhiteSpace(firstName) ? null : firstName,
            LastName = string.IsNullOrWhiteSpace(lastName) ? null : lastName,
        };
    }
}
