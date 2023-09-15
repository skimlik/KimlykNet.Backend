using System.Text;
using KimlykNet.Backend.Infrastructure.Configuration.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public static IServiceCollection AddAspNetIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<ITokenBuilder, TokenBuilder>();
        services.Configure<AuthenticationOptions>(configuration.GetSection(AuthenticationOptions.SectionName));

        var identityConnectionString = configuration.GetConnectionString("IdentityDb");
        services.AddDbContext<IdentityContext>(options =>
        {
            options.UseSqlServer(identityConnectionString);
        });

        services
            .AddIdentityCore<ApplicationUser>(
                opt =>
                {
                    opt.Password.RequiredLength = 6;
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireUppercase = true;
                    opt.User.RequireUniqueEmail = true;
                })
            .AddRoles<ApplicationRole>()
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<IdentityContext>();

        return services;
    }
}