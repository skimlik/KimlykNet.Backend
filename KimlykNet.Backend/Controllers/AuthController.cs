using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Contracts.Auth;
using KimlykNet.Services.Abstractions.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(INotificator notificator, ITokenBuilder tokenBuilder, IUserContextAccessor userContextAccessor)
    : ControllerBase
{
    [HttpPost]
    [Route("token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SecurityToken))]
    public async Task<IActionResult> GenerateTokenAsync([FromBody] TokenGenerationRequest request)
    {
        await notificator.NotifyAsync($"Authorization request received: {request.UserEmail}");
        var accessToken = await tokenBuilder.CreateAsync(request.UserEmail, request.Password, request.ClientId);
        if (accessToken?.Token == null)
        {
            await notificator.NotifyAsync($"Authorization rejected: {request.UserEmail}");
            return StatusCode(StatusCodes.Status401Unauthorized);
        }

        await notificator.NotifyAsync($"Token issued for {request.UserEmail}");
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