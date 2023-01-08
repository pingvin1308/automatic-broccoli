using AutoFixture;
using AutomaticBroccoli.DataAccess.Postgres.Entities;
using Xunit.Abstractions;

namespace AutomaticBroccoli.IntegrationTests;

public sealed class InboxControllerTests : IntegrationTestBase
{
    public InboxControllerTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
    }

    [Fact]
    public async Task CreateAttachment_ShouldReturnOkStatus()
    {
        try
        {
            var bytes = new byte[1024];
            var random = new Random();
            random.NextBytes(bytes);
            var fileName = "test.png";

            var attachmentLink = await Client.CreateAttachment(bytes, fileName);

            Assert.NotNull(attachmentLink);
            Assert.NotEmpty(attachmentLink);
        }
        finally
        {
            await CleanAsync();
        }
    }

    [Fact]
    public async Task GetAttachment_ShouldReturnOkStatus()
    {
        try
        {
            var path = "";
            var attachment = await Client.GetAttachment(path);
            Assert.NotNull(attachment);
        }
        finally
        {
            await CleanAsync();
        }
    }

    [Fact]
    public async Task Get_ShouldReturnOkStatus()
    {
        try
        {
            var fixture = new Fixture();
            var newOpenLoops = fixture.Build<OpenLoop>()
                .Without(x => x.User)
                .Without(x => x.UserId)
                .Without(x => x.CreatedDate)
                .CreateMany(42)
                .ToArray();
            var user = new User
            {
                Login = "test@mail.com",
                OpenLoops = newOpenLoops
            };
            Context.Users.Add(user);
            await Context.SaveChangesAsync();
            Context.ChangeTracker.Clear();

            var openLoops = await Client.GetOpenLoops(user.Id);
            Assert.NotNull(openLoops?.OpenLoops);
            Assert.NotEmpty(openLoops.OpenLoops);
            Assert.NotEqual(0, openLoops.Total);
        }
        finally
        {
            await CleanAsync();
        }
    }

    [Fact]
    public async Task Get_InvalidUserId_ShouldReturnOkStatusWithEmptyResult()
    {
        var userId = 0;
        var openLoops = await Client.GetOpenLoops(userId);
        Assert.NotNull(openLoops?.OpenLoops);
        Assert.Empty(openLoops.OpenLoops);
        Assert.True(openLoops.Total == 0);
    }
}
