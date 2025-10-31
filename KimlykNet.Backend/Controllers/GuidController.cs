using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using Swashbuckle.AspNetCore.Annotations;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class GuidController() : ControllerBase
{
    [HttpGet(Name = "GetNewGuid")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Produces new GUID")]
    public IActionResult Get()
    {
        return Ok(Guid.NewGuid());
    }
}
