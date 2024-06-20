using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace KimlykNet.Backend.Infrastructure.Configuration;

public class CorsMiddleware(
    RequestDelegate next,
    IOptions<CorsSettings> settings,
    ILogger<CorsMiddleware> logger)
{

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var values = new StringValues(settings.Value.AllowedOrigins);
            context.Response.Headers.AccessControlAllowOrigin = values;
            await next(context);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Failed to send AccessControlAllowOrigin header");
        }
    }
}