using System.Collections.Concurrent;
using Npgsql;

namespace AutomaticBroccoli.DbLoad.CLI;

public class MyCustomStressTest
{
    public static void Run()
    {
        var poolSize = 10;
        var connectionPool = new ConcurrentBag<NpgsqlConnection>();
        var semaphore = new Semaphore(poolSize, poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            var connection = new NpgsqlConnection(BroccoliDatabase.ConnectionString);
            connectionPool.Add(connection);
        }

        Parallel.For(0, 500_000, async (_) =>
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
                Console.WriteLine(userNotesInfo);
            }
            await connection.CloseAsync();
            connectionPool.Add(connection);
            semaphore.Release();
        });

        foreach (var connection in connectionPool)
        {
            connection.Dispose();
        }
    }
}