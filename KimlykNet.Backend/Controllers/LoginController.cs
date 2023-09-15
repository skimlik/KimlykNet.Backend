using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Contracts.Auth;

using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

public class LoginController : ControllerBase
{
    private readonly ITokenBuilder _tokenBuilder;

    public LoginController(ITokenBuilder tokenBuilder)
    {
        _tokenBuilder = tokenBuilder;
    }

    [HttpPost]
    [Route("token")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SecurityToken))]
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