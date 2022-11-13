using AutomaticBroccoli.CLI;
using AutomaticBroccoli.DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AutomaticBroccoli.IntegrationTests;

public class OpenLoopsControllerTests
{
    [Fact]
    public async Task Get_ShouldReturnOkStatus()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        OpenLoopsRepository.Add(new OpenLoop(Guid.NewGuid(), "Test note", DateTimeOffset.UtcNow));

        var response = await client.GetAsync("OpenLoops");
        response.EnsureSuccessStatusCode();
        
        Directory.Delete(OpenLoopsRepository.DataDirectory, true);
    }
}