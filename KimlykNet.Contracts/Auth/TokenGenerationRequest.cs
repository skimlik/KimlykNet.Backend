namespace KimlykNet.Contracts.Auth;

public class TokenGenerationRequest
{
    public string UserEmail { get; set; }

    public string Password { get; set; }

    public string ClientId { get; set; }
}