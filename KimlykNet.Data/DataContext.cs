using KimlykNet.Data.Abstractions.Entities;
using KimlykNet.Data.EntityConfiguration;

using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<UserNote> UserNotes { get; set; }

    public DbSet<SecretEntity> SecretMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserNoteConfiguration());
        modelBuilder.ApplyConfiguration(new SecretConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}