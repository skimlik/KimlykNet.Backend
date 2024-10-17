using KimlykNet.Data.Abstractions.Entities;
using KimlykNet.Data.EntityConfiguration;

using Microsoft.EntityFrameworkCore;

namespace KimlykNet.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<UserNote> UserNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserNoteConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}