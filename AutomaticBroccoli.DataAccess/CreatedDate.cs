namespace AutomaticBroccoli.DataAccess;

public readonly record struct CreatedDate
{
    public CreatedDate(DateTimeOffset createdDate)
    {
        if (createdDate == default)
        {
            throw new ArgumentException($"Invalid CreatedDate '{createdDate}'", paramName: nameof(createdDate));
        }

        Value = createdDate;
    }

    public DateTimeOffset Value { get; }

    public static implicit operator DateTimeOffset(CreatedDate createdDate) => createdDate.Value;
    public static implicit operator CreatedDate(DateTimeOffset value) => new(value);
}
