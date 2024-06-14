using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuidController(ILogger<GuidController> logger) : ControllerBase
{
    private readonly ILogger<GuidController> _logger = logger;

    [HttpGet(Name = "GetNewGuid")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Produces new GUID")]
    public IActionResult Get()
    {
        return Ok(Guid.NewGuid());
    }
}
