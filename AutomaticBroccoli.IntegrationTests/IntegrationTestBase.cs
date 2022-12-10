using AutomaticBroccoli.DataAccess.Postgres;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AutomaticBroccoli.IntegrationTests;

public abstract class IntegrationTestBase : IDisposable
{
    private readonly IServiceScope _scope;

    public IntegrationTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<IntegrationTestBase>()
            .Build();
        
        ConnectionString = configuration.GetConnectionString(nameof(AutomaticBroccoliDbContext));

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

                builder.UseConfiguration(configuration);
            });
        Client = factory.CreateClient();
        _scope = factory.Services.CreateScope();
        Context = _scope.ServiceProvider.GetRequiredService<AutomaticBroccoliDbContext>();
    }

    protected HttpClient Client { get; }
    protected string ConnectionString { get; }
    protected AutomaticBroccoliDbContext Context { get; }

    public void Dispose()
    {
        _scope.Dispose();
    }
}
