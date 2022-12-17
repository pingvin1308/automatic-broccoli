using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutomaticBroccoli.DataAccess.Postgres.Entities;

public class OpenLoop
{
    public OpenLoop()
    {
        CreatedDate = DateTimeOffset.UtcNow;
    }

    public Guid Id { get; set; }

    public string Note { get; set; }

    public DateTimeOffset CreatedDate { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public List<Attachment> Attachments { get; set; }
}

public sealed class OpenLoopEntityConfiguration : IEntityTypeConfiguration<OpenLoop>
{
    public void Configure(EntityTypeBuilder<OpenLoop> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Note).HasMaxLength(500);
    }
}
