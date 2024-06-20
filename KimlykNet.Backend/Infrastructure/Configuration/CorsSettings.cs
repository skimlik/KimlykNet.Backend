namespace KimlykNet.Backend.Infrastructure.Configuration;

public class CorsSettings
{
    public static readonly string SectionName = "Cors";
    
    public string PolicyName { get; set; }
    
    public string[] AllowedOrigins { get; set; }
}