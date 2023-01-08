namespace AutomaticBroccoli.Domain;

public interface IAuditableEntity
{
    DateTimeOffset UpdatedDate { get; init; }
    DateTimeOffset CreatedDate { get; init; }
}