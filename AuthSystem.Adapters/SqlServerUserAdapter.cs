using AuthSystem.Data;
using AuthSystem.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class SqlServerUserAdapter : IUserAdapter
    {
        private SqlConnection Connection { get; }

        public SqlServerUserAdapter(SqlConnection connection)
        {
            Connection = connection;
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
                    Hash = newUser.Base64PasswordHash,
                    Salt = newUser.Base64Salt
                });
        }

        public async Task<int> UpdateAsync(User newUserData)
        {
            return await Connection.ExecuteAsync(
                "UPDATE Users SET Username = @Username, Base64PasswordHash = @Hash, Base64Salt = @Salt WHERE Id = @Id",
                new
                {
                    Username = newUserData.Username, Hash = newUserData.Base64PasswordHash,
                    Salt = newUserData.Base64Salt, Id = newUserData.Id
                });
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

        public async Task<User?> ReadAsync(Guid id)
        {
            var users = await Connection.QueryAsync<User>("SELECT Id, Username, Base64PasswordHash, Base64Salt FROM Users WHERE Id = @Id", new { Id = id });
            if (users.Count() > 0)
            {
                return users.First();
            }
            else
            {
                return null;
            }
        }

        public async Task SaveAsync(User newData)
        {
            await Connection.ExecuteAsync(@"IF EXISTS (SELECT * FROM Users WHERE Id = @Id)
                                                UPDATE Users SET Username = @Username, Base64PasswordHash = @Hash, Base64Salt = @Salt WHERE Id = @Id
                                            ELSE
                                                INSERT Users (Id, Username, Base64PasswordHash, Base64Salt) VALUES (@Id, @Username, @Hash, @Salt)",
                                          new { newData.Id, newData.Username, Hash = newData.Base64PasswordHash, Salt = newData.Base64Salt });
        }

        #endregion
    }
}
