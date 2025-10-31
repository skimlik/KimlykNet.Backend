using System.Security.Claims;

using KimlykNet.Contracts.Auth;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class UserContextAccessor(IHttpContextAccessor httpContextAccessor) : IUserContextAccessor
{
    private const StringComparison OrdinalIgnoreCase = StringComparison.OrdinalIgnoreCase;

    public bool IsAuthenticated => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public UserInfo GetUserInfo()
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is null)
        {
            return null;
        }

        var identity = user.Identity;

        if (!IsAuthenticated)
        {
            return null;
        }

        string firstName = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.GivenName, OrdinalIgnoreCase))?.Value;
        string lastName = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Surname, OrdinalIgnoreCase))?.Value;

        return new UserInfo
        {
            Name = identity?.Name,
            Email = user.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email, OrdinalIgnoreCase))?.Value,
            FirstName = string.IsNullOrWhiteSpace(firstName) ? null : firstName,
            LastName = string.IsNullOrWhiteSpace(lastName) ? null : lastName,
        };
    }
}
