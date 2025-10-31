using KimlykNet.Data.Abstractions.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KimlykNet.Data.EntityConfiguration;

internal sealed class SecretConfiguration : IEntityTypeConfiguration<SecretEntity>
{
    public void Configure(EntityTypeBuilder<SecretEntity> builder)
    {
        builder.ToTable("SecretMessages", DataAccessConstants.DefaultSchema);
        
        builder.HasKey(k => k.Id);
        builder.Property(k => k.Id).ValueGeneratedNever().HasColumnName("id").IsRequired();
        builder.Property(k => k.CreatedOn).HasColumnName("created_on").IsRequired().HasDefaultValue(DateTimeOffset.UtcNow);
        builder.Property(k => k.Secret).HasColumnName("secret").HasMaxLength(256);
        builder.Property(k => k.ReceivedOn).HasColumnName("received_on");
        builder.Property(k => k.ReceivedBy).HasColumnName("received_by");
        builder.Property(k => k.CreatedBy).HasColumnName("created_by");
        builder.Property(k => k.SentTo).HasColumnName("sent_to");
    }
}