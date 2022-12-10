using System.Net.Http.Json;
using AutomaticBroccoli.API.Contracts;
using AutomaticBroccoli.CLI;
using AutomaticBroccoli.DataAccess;

namespace AutomaticBroccoli.IntegrationTests;

public class OpenLoopsControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task Get_ShouldReturnOkStatus()
    {
        OpenLoopsRepository.Add(new OpenLoop(Guid.NewGuid(), "Test note", DateTimeOffset.UtcNow));

        var response = await Client.GetAsync("v1/OpenLoops");
        response.EnsureSuccessStatusCode();
        
        Directory.Delete(OpenLoopsRepository.DataDirectory, true);
    }

    [Fact]
    public async Task Get_EmptyOpenLoops_ShouldReturnOkStatus()
    {
        var response = await Client.GetAsync("v1/OpenLoops");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Create_FirstOpenLoop_ShouldReturnOkStatus()
    {
        var request = new CreateOpenLoopRequest
        {
            Note = "First open loop"
        };

        var response = await Client.PostAsJsonAsync("v1/OpenLoops", request);
        response.EnsureSuccessStatusCode();
        
        Directory.Delete(OpenLoopsRepository.DataDirectory, true);
    }

    [Fact]
    public async Task Create_AdditionalOpenLoop_ShouldReturnOkStatus()
    {
        OpenLoopsRepository.Add(new OpenLoop(Guid.NewGuid(), "First open loop", DateTimeOffset.UtcNow));
        var request = new CreateOpenLoopRequest
        {
            Note = "Additional open loop"
        };

        var response = await Client.PostAsJsonAsync("v1/OpenLoops", request);
        response.EnsureSuccessStatusCode();
        
        Directory.Delete(OpenLoopsRepository.DataDirectory, true);
    }
}