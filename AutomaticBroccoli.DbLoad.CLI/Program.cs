// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using AutomaticBroccoli.DbLoad.CLI;
using NBomber.Contracts;
using NBomber.CSharp;
using Npgsql;

var poolSize = 100;
var connectionPool = new ConcurrentBag<NpgsqlConnection>();
var semaphore = new Semaphore(poolSize, poolSize);
for (int i = 0; i < poolSize; i++)
{
    var connection = new NpgsqlConnection(BroccoliDatabase.ConnectionString);
    connectionPool.Add(connection);
}

var step = Step.Create("fetch user's notes",
    execute: async context =>
    {
        semaphore.WaitOne();
        NpgsqlConnection? connection = null;
        do { } while (!connectionPool.TryTake(out connection));

        await connection.OpenAsync();

        var randomUserId = Random.Shared.Next(1, 1800);
        var userNotes = await BroccoliDatabase.GetUserNotes(connection, $"test{randomUserId}@mail.ru");
        if (userNotes.Any())
        {
            var user = userNotes.First();
            var userNotesInfo = $"UserId: '{user.UserId}', Login: '{user.Login}', Count:'{userNotes.Length}'";
            // Console.WriteLine(userNotesInfo);
        }

        await connection.CloseAsync();
        connectionPool.Add(connection);
        semaphore.Release();

        return Response.Ok();
    });

var scenario = ScenarioBuilder
    .CreateScenario("postgres", step)
    .WithWarmUpDuration(TimeSpan.FromSeconds(5))
    .WithLoadSimulations(
        Simulation.InjectPerSec(rate: 1000, during: TimeSpan.FromSeconds(120))
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .Run();