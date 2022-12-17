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

        var connectionString = configuration.GetConnectionString(nameof(AutomaticBroccoliDbContext));
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            var errorMessage = "connectionString cannot be null or empty. Please make sure that you have configured ConnectionString in user secrets for test project.";
            throw new ArgumentNullException(errorMessage);
        }

        ConnectionString = connectionString;

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
