using AutomaticBroccoli.Domain;

namespace AutomaticBroccoli.UnitTests;

public class NoteTests
{
    [Fact]
    public void Create_ShouldReturnNewNote()
    {
        // arrange
        var noteValue = "test note";

        // act
        var note = Note.Create(noteValue);

        // assert
        Assert.NotNull(note.Value);
    }
}