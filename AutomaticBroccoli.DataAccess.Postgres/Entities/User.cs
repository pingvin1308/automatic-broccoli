using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutomaticBroccoli.DataAccess.Postgres.Entities;

public class User
{
    public User()
    {
        CreatedDate = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public string Login { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public ICollection<OpenLoop> OpenLoops { get; set; }
}

public sealed class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Login).HasMaxLength(50);
        builder.HasMany(x => x.OpenLoops)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);
    }
}