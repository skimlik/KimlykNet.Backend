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
        builder.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
        builder.Property(e => e.NoteId).HasColumnName("note_id").IsRequired();
        builder.Property(e => e.Note).HasColumnName("note").HasMaxLength(4096);
        builder.Property(e => e.DateCreated).HasColumnName("created_at").IsRequired().HasDefaultValue(DateTimeOffset.UtcNow);
        builder.Property(e => e.DateModified).HasColumnName("updated_at").HasDefaultValue(DateTimeOffset.UtcNow);

        builder.HasIndex(i => i.UserId);
    }
}