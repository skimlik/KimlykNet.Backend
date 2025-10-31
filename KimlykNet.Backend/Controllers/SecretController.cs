using KimlykNet.Backend.Infrastructure.Auth;
using KimlykNet.Contracts.Auth;
using KimlykNet.Data.Abstractions;
using KimlykNet.Data.Abstractions.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class SecretController(IIdEncoder encoder, ISecretMessageRepository repository, IUserContextAccessor userContextAccessor) : ControllerBase
{
    [HttpGet("{messageId}", Name = "GetSecretAction")]
    public async Task<IActionResult> GetAsync([FromRoute] string messageId, CancellationToken cancellationToken)
    {
        var id = encoder.Decode(messageId);
        if (Guid.TryParse(id, out var guid))
        {
            var userInfo = userContextAccessor.GetUserInfo();
            var secret = await repository.GetSecretAsync(guid, userInfo?.Email,  cancellationToken);
            if (secret is null)
            {
                return NotFound();
            }
            return Ok(
                new
                {
                    secret.Secret,
                });
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSecretModel model, CancellationToken cancellationToken)
    {
        var userInfo = userContextAccessor.GetUserInfo();
        var secretId = await repository.CreateSecretAsync(model.Value, userInfo?.Email, cancellationToken);
        var messageId = encoder.Encode(secretId.ToString());
        return CreatedAtRoute("GetSecretAction", new { messageId }, messageId);
    }
}

public class CreateSecretModel
{
    public string Value { get; set; }
}