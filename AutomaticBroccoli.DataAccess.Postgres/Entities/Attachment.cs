using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutomaticBroccoli.DataAccess.Postgres.Entities;

public class Attachment
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Size { get; set; }

    public string Link { get; set; }

    public OpenLoop OpenLoop { get; set; }

    public Guid OpenLoopId { get; set; }
}

public sealed class AttachmentEntityConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(1000);
        builder.Property(x => x.Link).HasMaxLength(2000);
    }
}
