namespace AutomaticBroccoli.DataAccess;

public readonly record struct Note
{
    public Note(string note)
    {
        if (string.IsNullOrWhiteSpace(note))
        {
            throw new ArgumentException("Note cannot be null or whitespace", paramName: nameof(note));
        }

        Value = note;
    }

    public string Value { get; }

    public static implicit operator string(Note note) => note.Value;
    public static implicit operator Note(string value) => new(value);
}
