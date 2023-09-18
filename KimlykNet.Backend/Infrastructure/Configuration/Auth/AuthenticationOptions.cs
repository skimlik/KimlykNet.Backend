namespace KimlykNet.Backend.Infrastructure.Configuration.Auth;

public class AuthenticationOptions
{
    public static readonly string SectionName = "KimlykNet:Authentication";

    public string Issuer { get; set; }

    public string[] Audiences { get; set; }

    public string SigningKey { get; set; }

    public TimeSpan Lifetime { get; set; }
}