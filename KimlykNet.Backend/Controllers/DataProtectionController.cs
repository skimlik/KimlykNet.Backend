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
        string encode = encoder.Encode(secretValue.Value);
        return Ok(new ProtectedValue {  Value = encode });
    }
    
    [HttpPost("decode")]
    public IActionResult Decode([FromBody]ProtectedValue secretValue)
    {
        string decode = encoder.Decode(secretValue.Value);
        return Ok(new ProtectedValue { Value = decode});
    }

    public class ProtectedValue
    {
        public string Value { get; set; }
    }
}