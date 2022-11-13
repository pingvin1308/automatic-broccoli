using AutomaticBroccoli.DataAccess;

namespace AutomaticBroccoli.UnitTests.DataAccess;

public class OpenLoopTests
{
    [Fact]
    public void Create_ReturnNewOpenLoop()
    {
        // arrange
        var openLoopId = Guid.NewGuid();
        var note = "Test note";
        var createdDate = DateTimeOffset.Now;

        // act
        var openLoop = new OpenLoop(openLoopId, note, createdDate);

        // assert
        Assert.NotNull(openLoop);
        Assert.False(string.IsNullOrWhiteSpace(openLoop.Note));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_InvalidNote_Return(string invalidNote)
    {
        // arrange
        var openLoopId = Guid.NewGuid();
        var createdDate = DateTimeOffset.Now;

        // act - assert
        Assert.Throws<ArgumentException>(() => new OpenLoop(openLoopId, invalidNote, createdDate));
    }

    [Fact]
    public void Create_InvalidCreatedDate_ReturnNewOpenLoop()
    {
        // arrange
        var openLoopId = Guid.NewGuid();
        var note = "Test note";
        var createdDate = default(DateTimeOffset);

        // act - assert
        Assert.Throws<ArgumentException>(() => new OpenLoop(openLoopId, note, createdDate));
    }
}