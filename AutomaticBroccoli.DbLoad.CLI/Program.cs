// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using Npgsql;

Console.WriteLine("Hello, World!");

var connectionString = "User ID=postgres;Password=example123;Server=localhost;Port=15432;Database=AutomaticBroccoliDb;";
var poolSize = 10;
var connectionPool = new ConcurrentBag<NpgsqlConnection>();
var semaphore = new Semaphore(poolSize, poolSize);
for (int i = 0; i < poolSize; i++)
{
    var connection = new NpgsqlConnection(connectionString);
    connectionPool.Add(connection);
}

Parallel.For(0, 500_000, new ParallelOptions { MaxDegreeOfParallelism = 8 }, async (_) =>
{
    semaphore.WaitOne();
    NpgsqlConnection? connection = null;
    do {} while (!connectionPool.TryTake(out connection));

    await connection.OpenAsync();
    var randomUserId = Random.Shared.Next(1, 800);
    var user = await GetUsers(connection, $"test{randomUserId}@mail.ru");
    Console.WriteLine(user.FirstOrDefault());
    await connection.CloseAsync();
    connectionPool.Add(connection);
    semaphore.Release();
});

foreach (var connection in connectionPool)
{
    connection.Dispose();
}

// var sql = @"SELECT COUNT(1) FROM ""Users""";

// using var command = new NpgsqlCommand(sql, connection);
// command.Parameters.Clear();

async Task<long> GetUsersTotal(NpgsqlConnection connection)
{
    if (connection.State == System.Data.ConnectionState.Closed)
    {
        throw new ArgumentException("Connection cannot be closed");
    }

    var sql = @"SELECT COUNT(1) FROM ""Users""";

    using var command = new NpgsqlCommand(sql, connection);
    command.Parameters.Clear();

    var count = (long?)await command.ExecuteScalarAsync() ?? 0;
    return count;
}

async Task<User[]> GetUsers(NpgsqlConnection connection, string? login = null)
{
    if (connection.State == System.Data.ConnectionState.Closed)
    {
        throw new ArgumentException("Connection cannot be closed");
    }

    var sql = @"
        SELECT ""Id"", ""Login"", ""CreatedDate""
        FROM ""Users""
        WHERE 1=1";

    using var command = new NpgsqlCommand(sql, connection);
    command.Parameters.Clear();

    if (login != null)
    {
        command.CommandText += @" AND ""Login"" = @Login";
        command.Parameters.Add(new NpgsqlParameter("@Login", login));
    }

    using var reader = await command.ExecuteReaderAsync();

    if (!reader.HasRows)
    {
        return Array.Empty<User>();
    }

    var users = new List<User>();
    while (await reader.ReadAsync())
    {
        var user = new User(
            Id: (int)reader.GetValue(0),
            Login: (string)reader[nameof(User.Login)],
            CreatedDate: reader.GetDateTime(2)
        );
        users.Add(user);
    }

    return users.ToArray();
}

record User(int Id, string Login, DateTimeOffset CreatedDate);