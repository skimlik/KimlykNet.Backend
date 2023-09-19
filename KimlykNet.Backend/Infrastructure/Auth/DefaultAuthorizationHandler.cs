using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

namespace KimlykNet.Backend.Infrastructure.Auth;

public class DefaultAuthorizationHandler : AuthorizationHandler<IAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        var user = context.User;
        var identity = user.Identity as ClaimsIdentity;
        if (identity?.IsAuthenticated ?? false)
        {
            return requirement switch
            {
                _ => AuthorizeAsync(context, requirement)
            };
        }

        return AccessDeniedAsync(context);
    }

    private Task AuthorizeAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private Task AccessDeniedAsync(AuthorizationHandlerContext context)
    {
        context.Fail(new AuthorizationFailureReason(this, "Access Denied"));
        return Task.CompletedTask;
    }
}
