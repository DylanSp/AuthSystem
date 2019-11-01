using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using Npgsql;
using System;
using System.Threading.Tasks;
using AuthSystem.Interfaces;

namespace AuthSystem.Adapters
{
    public class PostgresUserAdapter : IUserAdapter
    {
        private IPostgresConnectionContext ConnectionContext { get; }

        public PostgresUserAdapter(IPostgresConnectionContext connectionContext)
        {
            ConnectionContext = connectionContext;
        }

        public async Task<int> CreateUserAsync(User newUser)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"INSERT INTO Users (Id, Username, SaltedHash)
                                        VALUES (@Id, @Username, @SaltedHash)";
                command.Parameters.AddWithValue("Id", newUser.Id.Value);
                command.Parameters.AddWithValue("Username", newUser.Username.Value);
                command.Parameters.AddWithValue("SaltedHash", newUser.SaltedHashedPassword.Value);

                try
                {
                    var numCreated = await command.ExecuteNonQueryAsync();
                    return numCreated;
                }
                catch (NpgsqlException)
                {
                    // in case query fails (probably due to duplicate username)
                    return 0;
                }
            }
        }

        public async Task<User?> GetUserByIdAsync(UserId userId)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"SELECT Id, Username, SaltedHash
                                        FROM Users
                                        WHERE Id = @Id";
                command.Parameters.AddWithValue("Id", userId.Value);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var readId = new UserId((Guid)reader["Id"]);
                    var username = new Username((string)reader["Username"]);
                    var saltedHash = new SaltedHashedPassword((string)reader["SaltedHash"]);
                    return new User(readId, username, saltedHash);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<User?> GetUserByUsernameAsync(Username username)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"SELECT Id, Username, SaltedHash
                                        FROM Users
                                        WHERE Username = @Username";
                command.Parameters.AddWithValue("Username", username.Value);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var id = new UserId((Guid) reader["Id"]);
                        var readUsername = new Username((string) reader["Username"]);
                        var saltedHash = new SaltedHashedPassword((string) reader["SaltedHash"]);
                        return new User(id, readUsername, saltedHash);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<int> UpdateUserAsync(User newUser)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"UPDATE Users
                                        SET Username = @Username, SaltedHash = @SaltedHash
                                        WHERE Id = @Id";
                command.Parameters.AddWithValue("Id", newUser.Id.Value);
                command.Parameters.AddWithValue("Username", newUser.Username.Value);
                command.Parameters.AddWithValue("SaltedHash", newUser.SaltedHashedPassword.Value);

                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
