namespace AutomaticBroccoli.DataAccess;

public record OpenLoop
{
    public OpenLoop(Guid id, Note note, CreatedDate createdDate)
    {
        Id = id;
        Note = note;
        CreatedDate = createdDate;
    }

    public Guid Id { get; init; }
    public Note Note { get; init; }
    public CreatedDate CreatedDate { get; init; }
}