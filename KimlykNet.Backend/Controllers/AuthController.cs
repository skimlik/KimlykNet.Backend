using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenBuilder _tokenBuilder;
    private readonly IUserContextAccessor _userContextAccessor;

    public AuthController(ITokenBuilder tokenBuilder, IUserContextAccessor userContextAccessor)
    {
        _tokenBuilder = tokenBuilder;
        _userContextAccessor = userContextAccessor;
    }

    [HttpPost]
    [Route("token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SecurityToken))]
    public async Task<IActionResult> GenerateTokenAsync([FromBody] TokenGenerationRequest request)
    {
        var accessToken = await _tokenBuilder.CreateAsync(request.UserEmail, request.Password, request.ClientId);
        if (accessToken?.Token == null)
        {
            return StatusCode(StatusCodes.Status401Unauthorized);
        }
        return Ok(accessToken);
    }

    [HttpGet]
    [Route("userInfo")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserInfo))]
    [Authorize]
    public IActionResult GetUserInfo()
    {
        var user = _userContextAccessor.GetUserInfo();

        if (user is null)
        {
            return BadRequest();
        }

        return Ok(user);
    }
}