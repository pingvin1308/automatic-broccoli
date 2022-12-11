// See https://aka.ms/new-console-template for more information
using Npgsql;

Console.WriteLine("Hello, World!");

var connectionString = "User ID=postgres;Password=example123;Server=localhost;Port=15432;Database=AutomaticBroccoliDb;";
using var connection = new NpgsqlConnection(connectionString);

await connection.OpenAsync();

var count = await GetUsersTotal(connection);

Console.WriteLine(count);

var users = await GetUsers(connection);

Console.WriteLine(string.Join("\r\n", users.Select(x => x.ToString())));

var user = await GetUsers(connection, "test995@mail.ru");
Console.WriteLine(user.FirstOrDefault());

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