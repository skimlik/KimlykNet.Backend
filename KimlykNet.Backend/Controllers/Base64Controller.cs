using System.Text;

using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace KimlykNet.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class Base64Controller : ControllerBase
{
    [HttpPost("to")]
    public IActionResult ToBase64([FromBody] Base64Request base64)
    {
        return Ok(new { Converted = Convert.ToBase64String(Encoding.UTF8.GetBytes(base64.InputText)) });
}

    [HttpPost("from")]
    public IActionResult FromBase64([FromBody] Base64Request base64)
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.InputText.Length]);
        if (Convert.TryFromBase64String(base64.InputText, buffer, out var _))
        {
            return Ok(new { Converted = Encoding.UTF8.GetString(Convert.FromBase64String(base64.InputText)) });
        }

        return BadRequest("Not a base64 string");
    }
    
    public sealed class Base64Request
    {
        public string InputText { get; set; }
    }
}