using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Contracts.Auth;
using KimlykNet.Services;
using KimlykNet.Services.Abstractions.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(NotificationChannel notificator, ITokenBuilder tokenBuilder, IUserContextAccessor userContextAccessor)
    : ControllerBase
{
    [HttpPost]
    [Route("token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SecurityToken))]
    public async Task<IActionResult> GenerateTokenAsync([FromBody] TokenGenerationRequest request, CancellationToken cancellationToken)
    {
        notificator.ScheduleNotification(
            new ApplicationNotification { Text = $"Authentication request received {request.UserEmail}"},
            cancellationToken);

        var accessToken = await tokenBuilder.CreateAsync(request.UserEmail, request.Password, request.ClientId, cancellationToken);
        if (accessToken?.Token == null)
        {
            notificator.ScheduleNotification(
                new ApplicationNotification { Text = $"Authentication request failed {request.UserEmail}"},
                cancellationToken);

            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        notificator.ScheduleNotification(
            new ApplicationNotification { Text = $"Authentication request succeed {request.UserEmail}"},
            cancellationToken);

        return Ok(accessToken);
    }

    [HttpGet]
    [Route("userInfo")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfo))]
    [Authorize]
    public IActionResult GetUserInfo()
    {
        var user = userContextAccessor.GetUserInfo();

        if (user is null)
        {
            return BadRequest();
        }

        return Ok(user);
    }
}