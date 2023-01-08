namespace AutomaticBroccoli.Domain.Organization;

public sealed record Trash : IAuditableEntity
{
    public Guid Id { get; init; }

    public Note Description { get; init; }

    public DateTimeOffset UpdatedDate { get; init; }

    public DateTimeOffset CreatedDate { get; init; }
}