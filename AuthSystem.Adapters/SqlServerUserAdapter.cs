using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    // TODO - rework this, maybe put password info in separate table?
    public class SqlServerUserAdapter : IUserAdapter
    {
        private SqlConnection Connection { get; }

        public SqlServerUserAdapter(SqlConnection connection)
        {
            Connection = connection;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            var users = await Connection.QueryAsync<User>("SELECT Id, Username, Base64PasswordHash, Base64Salt FROM Users WHERE Id = @Id", new { Id = userId });
            if (users.Count() > 0)
            {
                return users.First();
            }
            else
            {
                return null;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var users = await Connection.QueryAsync<User>("SELECT Id, Username, Base64PasswordHash, Base64Salt FROM Users WHERE Username = @Username", new { Username = username });
            if (users.Count() > 0)
            {
                return users.First();
            }
            else
            {
                return null;
            }
        }

        // allows exceptions to propagate upwards
        public async Task CreateAsync(User newUser)
        {
            await Connection.ExecuteAsync(
                "INSERT Users (Id, Username, Base64PasswordHash, Base64Salt) VALUES (@Id, @Username, @Hash, @Salt)",
                new
                {
                    Id = newUser.Id,
                    Username = newUser.Username,
                    Hash = newUser.HashedPassword.Base64PasswordHash,
                    Salt = newUser.HashedPassword.Base64Salt
                });
        }

        public async Task<int> UpdateAsync(User newUser)
        {
            return await Connection.ExecuteAsync(
                "UPDATE Users SET Username = @Username, Base64PasswordHash = @Hash, Base64Salt = @Salt WHERE Id = @Id",
                new
                {
                    Username = newUser.Username,
                    Hash = newUser.HashedPassword.Base64PasswordHash,
                    Salt = newUser.HashedPassword.Base64Salt,
                    Id = newUser.Id
                });
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            var usersWithUsername =
                await Connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Users WHERE Username = @Username",
                    new {Username = username});
            return usersWithUsername == 0;
        }


        #region old stuff

        public async Task DeleteAsync(Guid id)
        {
            await Connection.ExecuteAsync("DELETE FROM Users WHERE Id = @Id", new { Id = id });
        }

        public async Task<IEnumerable<User>> ReadAllAsync()
        {
            return await Connection.QueryAsync<User>("SELECT Id, Username, Base64PasswordHash, Base64Salt FROM Users");
        }

        #endregion
    }
}
