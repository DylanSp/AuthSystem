using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class PostgresSessionCookieAdapter : ISessionCookieAdapter
    {
        private IPostgresConnectionContext ConnectionContext { get; }

        public PostgresSessionCookieAdapter(IPostgresConnectionContext connectionContext)
        {
            ConnectionContext = connectionContext;
        }

        public async Task<int> CreateSessionCookieAsync(SessionCookie cookie)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"INSERT INTO SessionCookies (Id, UserId)
                                        VALUES (@Id, @UserId)";
                command.Parameters.AddWithValue("Id", cookie.Id.Value);
                command.Parameters.AddWithValue("UserId", cookie.UserId.Value);

                var numInserted = await command.ExecuteNonQueryAsync();
                return numInserted;
            }
        }

        public async Task<UserId?> GetUserForCookieAsync(SessionCookieId cookieId)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"SELECT UserId
                                        FROM SessionCookies
                                        WHERE Id = @Id";
                command.Parameters.AddWithValue("Id", cookieId.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var userId = new UserId((Guid)reader["UserId"]);
                        return userId;
                    } 
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task DeleteSessionsForUserAsync(UserId userId)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"DELETE FROM SessionCookies
                                        WHERE UserId = @UserId";
                command.Parameters.AddWithValue("UserId", userId.Value);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
