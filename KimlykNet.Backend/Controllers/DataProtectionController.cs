using KimlykNet.Data.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
[Authorize]
public class DataProtectionController(IIdEncoder encoder) : ControllerBase
{
    [HttpPost("encode")]
    public IActionResult Encode([FromBody]ProtectedValue secretValue)
    {
        return Ok(encoder.Encode(secretValue.Value));
    }
    
    [HttpPost("decode")]
    public IActionResult Decode([FromBody]ProtectedValue secretValue)
    {
        return Ok(encoder.Decode(secretValue.Value));
    }

    public class ProtectedValue
    {
        public string Value { get; set; }
    }
}