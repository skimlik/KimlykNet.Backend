using KimlykNet.Data.Abstractions.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KimlykNet.Data.EntityConfiguration;

internal class UserNoteConfiguration : IEntityTypeConfiguration<UserNote>
{
    public void Configure(EntityTypeBuilder<UserNote> builder)
    {
        builder.ToTable("UserNotes", DataAccessConstants.DefaultSchema);

        builder.HasKey(k => k.Id);
        builder.Property(e => e.Id).HasColumnName("id").IsRequired().UseIdentityByDefaultColumn();
        builder.Property(e => e.User).HasColumnName("user").IsRequired().HasMaxLength(256);
        builder.Property(e => e.Title).HasColumnName("title").IsRequired().HasMaxLength(256);
        builder.Property(e => e.Note).HasColumnName("note").HasMaxLength(4096);
        builder.Property(e => e.DateCreated).HasColumnName("created_at").IsRequired().HasDefaultValue(DateTimeOffset.UtcNow);
        builder.Property(e => e.DateModified).HasColumnName("updated_at").HasDefaultValue(DateTimeOffset.UtcNow);

        builder.HasIndex(i => i.DateCreated);
    }
}