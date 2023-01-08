using Npgsql;

namespace AutomaticBroccoli.DbLoad.CLI;

/// <summary>
/// Standard queries for database.
/// </summary>
public static class BroccoliDatabase
{
    public const string ConnectionString = "User ID=postgres;Password=example123;Server=localhost;Port=15432;Database=AutomaticBroccoliDb;";

    public static async Task<long> GetUsersTotal(NpgsqlConnection connection)
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

    public static async Task<User[]> GetUsers(NpgsqlConnection connection, string? login = null)
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

    public static async Task<OpenLoop[]> GetUserNotes(NpgsqlConnection connection, string? login = null)
    {
        if (connection.State == System.Data.ConnectionState.Closed)
        {
            throw new ArgumentException("Connection cannot be closed");
        }

        var sql = @"
        SELECT 
            u.""Id"", 
            u.""Login"", 
            ol.""Note"", 
            ol.""CreatedDate""
        FROM ""Users"" u
        INNER JOIN ""OpenLoops"" ol ON ol.""UserId"" = u.""Id""
        WHERE 1=1";

        using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Clear();

        if (login != null)
        {
            command.CommandText += @" AND u.""Login"" = @Login";
            command.Parameters.Add(new NpgsqlParameter("@Login", login));
        }

        using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
        {
            return Array.Empty<OpenLoop>();
        }

        var openLoops = new List<OpenLoop>();
        while (await reader.ReadAsync())
        {
            var openLoop = new OpenLoop(
                UserId: reader.GetInt32(0),
                Login: reader.GetString(1),
                Note: reader.GetString(2),
                CreatedDate: reader.GetDateTime(3)
            );
            openLoops.Add(openLoop);
        }

        return openLoops.ToArray();
    }
}