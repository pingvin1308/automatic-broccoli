using AutomaticBroccoli.DataAccess.Postgres;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Respawn.Graph;

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
            throw new ArgumentException(errorMessage);
        }

        ConnectionString = connectionString;

        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {

                builder.UseConfiguration(configuration);
            });

        var httpClient = factory.CreateClient();
        Client = new AutomaticBroccoliApiClient(httpClient);
        _scope = factory.Services.CreateScope();
        Context = _scope.ServiceProvider.GetRequiredService<AutomaticBroccoliDbContext>();
    }

    protected string ConnectionString { get; }
    protected AutomaticBroccoliDbContext Context { get; }
    protected AutomaticBroccoliApiClient Client { get; }

    public void Dispose()
    {
        _scope.Dispose();
    }

    protected async Task CleanAsync()
    {
        using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        var respawner = await Respawner.CreateAsync(
            connection,
            new RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                    "__EFMigrationsHistory",
                },
                SchemasToInclude = new[]
                {
                    "public"
                },
                DbAdapter = DbAdapter.Postgres
            });

        await respawner.ResetAsync(connection);
    }
}
