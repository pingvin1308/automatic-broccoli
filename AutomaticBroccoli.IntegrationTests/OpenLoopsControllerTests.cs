using System.Net.Http.Json;
using AutomaticBroccoli.API.Contracts;
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

    [Fact]
    public async Task Get_EmptyOpenLoops_ShouldReturnOkStatus()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        var response = await client.GetAsync("OpenLoops");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Create_FirstOpenLoop_ShouldReturnOkStatus()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var request = new CreateOpenLoopRequest
        {
            Note = "First open loop"
        };

        var response = await client.PostAsJsonAsync("OpenLoops", request);
        response.EnsureSuccessStatusCode();
        
        Directory.Delete(OpenLoopsRepository.DataDirectory, true);
    }

    [Fact]
    public async Task Create_AdditionalOpenLoop_ShouldReturnOkStatus()
    {
        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();

        OpenLoopsRepository.Add(new OpenLoop(Guid.NewGuid(), "First open loop", DateTimeOffset.UtcNow));
        var request = new CreateOpenLoopRequest
        {
            Note = "Additional open loop"
        };

        var response = await client.PostAsJsonAsync("OpenLoops", request);
        response.EnsureSuccessStatusCode();
        
        Directory.Delete(OpenLoopsRepository.DataDirectory, true);
    }
}