using AutoFixture;
using AutomaticBroccoli.DataAccess.Postgres.Entities;

namespace AutomaticBroccoli.IntegrationTests;

public sealed class InboxControllerTests : IntegrationTestBase
{
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
