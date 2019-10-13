using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class PostgresUserAdapter : IUserAdapter
    {
        private NpgsqlConnection Connection { get; }

        public PostgresUserAdapter(NpgsqlConnection connection)
        {
            Connection = connection;
        }

        public async Task CreateAsync(User newUser)
        {
            var insertUserQuery = @"INSERT INTO Users (Id, Username, Base64PasswordHash, Base64Salt)
                                    VALUES (@Id, @Username, @Hash, @Salt)";
            using (var command = new NpgsqlCommand(insertUserQuery, Connection))
            {
                command.Parameters.AddWithValue("Id", newUser.Id.Value);
                command.Parameters.AddWithValue("Username", newUser.Username.Value);
                command.Parameters.AddWithValue("Hash", newUser.HashedPassword.Base64PasswordHash.Value);
                command.Parameters.AddWithValue("Salt", newUser.HashedPassword.Base64Salt.Value);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<User?> GetUserByIdAsync(UserId userId)
        {
            var getUserQuery = @"SELECT Id, Username, Base64PasswordHash, Base64Salt
                                 FROM Users
                                 WHERE Id = @Id";
            using (var command = new NpgsqlCommand(getUserQuery, Connection))
            {
                command.Parameters.AddWithValue("Id", userId.Value);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var readId = (Guid) reader["Id"];
                    var username = (string)reader["Username"];
                    var hash = (string)reader["Base64PasswordHash"];
                    var salt = (string)reader["Base64Salt"];
                    return new User(UserId.From(readId), Username.From(username), new HashedPassword(Base64Hash.From(hash), Base64Salt.From(salt)));
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<User?> GetUserByUsernameAsync(Username username)
        {
            var getUserQuery = @"SELECT Id, Username, Base64PasswordHash, Base64Salt
                                 FROM Users
                                 WHERE Username = @Username";
            using (var command = new NpgsqlCommand(getUserQuery, Connection))
            {
                command.Parameters.AddWithValue("Username", username.Value);
                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var id = (Guid)reader["Id"];
                    var readUsername = (string)reader["Username"];
                    var hash = (string)reader["Base64PasswordHash"];
                    var salt = (string)reader["Base64Salt"];
                    return new User(UserId.From(id), Username.From(readUsername), new HashedPassword(Base64Hash.From(hash), Base64Salt.From(salt)));
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<bool> IsUsernameUniqueAsync(Username username)
        {
            var isUniqueQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            using (var command = new NpgsqlCommand(isUniqueQuery, Connection))
            {
                command.Parameters.AddWithValue("Username", username.Value);
                var result = (long) await command.ExecuteScalarAsync();
                return result <= 1;
            }
        }

        public async Task<int> UpdateAsync(User newUser)
        {
            throw new NotImplementedException();
        }
    }
}
