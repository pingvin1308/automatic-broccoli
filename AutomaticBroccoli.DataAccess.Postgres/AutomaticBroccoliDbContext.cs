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
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AutomaticBroccoliDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
