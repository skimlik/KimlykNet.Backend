namespace KimlykNet.Backend.Infrastructure.Auth;

public class SecurityToken
{
    public string Token { get; set; }

    public DateTime Expiration { get; set; }
}
