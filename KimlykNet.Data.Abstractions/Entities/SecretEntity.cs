namespace KimlykNet.Data.Abstractions.Entities;

public class SecretEntity
{
    public Guid Id { get; set; }

    public string Secret { get; set; }

    public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

    public string CreatedBy { get; set; }

    public string SentTo { get; set; }

    public DateTimeOffset? ReceivedOn { get; set; }

    public string ReceivedBy { get; set; }
}