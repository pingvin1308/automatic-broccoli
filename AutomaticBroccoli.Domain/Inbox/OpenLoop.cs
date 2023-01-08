namespace AutomaticBroccoli.Domain;
public sealed record OpenLoop : IAuditableEntity
{
    public Guid Id { get; init; }

    public Note Note { get; init; }

    public DateTimeOffset UpdatedDate { get; init; }

    public DateTimeOffset CreatedDate { get; init; }
}