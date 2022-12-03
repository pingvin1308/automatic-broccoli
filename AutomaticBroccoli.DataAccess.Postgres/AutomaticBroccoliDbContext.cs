using AutomaticBroccoli.DataAccess.Postgres.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutomaticBroccoli.DataAccess.Postgres;

public class AutomaticBroccoliDbContext : DbContext
{
    public AutomaticBroccoliDbContext(DbContextOptions<AutomaticBroccoliDbContext> options) : base(options)
    {
    }

    public DbSet<OpenLoop> OpenLoops { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserNotes> UserNotes { get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutomaticBroccoliDbContext).Assembly);

        modelBuilder.Entity<UserNotes>()
            .HasNoKey()
            .Metadata.SetIsTableExcludedFromMigrations(true);

        base.OnModelCreating(modelBuilder);
    }
}
