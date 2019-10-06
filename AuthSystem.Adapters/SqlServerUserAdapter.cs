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

        public async Task CreateAsync(User newUser)
        {
            // how to handle case where user already exists?
        }

        public async Task UpdateAsync(User newUserData)
        {
            // how to handle case where user doesn't exist?
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
