using System.Text;
using KimlykNet.Backend.Infrastructure.Configuration.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace KimlykNet.Backend.Infrastructure.Auth;

internal static class AuthenticationRegistrationExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        var settings = configuration.GetSection(AuthenticationOptions.SectionName).Get<AuthenticationOptions>();

        if (settings is null)
        {
            throw new ApplicationException("Authentication configuration section is not configured");
        }

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey))
                };
            });

        return services;
    }
}