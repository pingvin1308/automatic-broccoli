using System.Diagnostics.CodeAnalysis;

namespace AutomaticBroccoli.Domain;

public readonly record struct Note
{
    public const int MaxNoteLength = 500;
    private readonly string? _value;

    public static readonly Note Empty;

    private Note(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("note cannot be null or whitespace.", nameof(value));
        }

        _value = value;
    }

    public readonly string Value => _value ?? string.Empty;

    public static Note Create(string value)
    {
        return new Note(value);
    }
}
