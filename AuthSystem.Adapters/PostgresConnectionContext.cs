using AuthSystem.Interfaces;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class PostgresConnectionContext : IPostgresConnectionContext
    {
        private NpgsqlConnection Connection { get; }

        public PostgresConnectionContext(string connectionString)
        {
            // validate connection string with the builder
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            Connection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);
        }

        public async Task<NpgsqlCommand> CreateCommandAsync()
        {
            if (Connection.State != ConnectionState.Open)
            {
                await Connection.OpenAsync();
            }
            return Connection.CreateCommand();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
            }
        }
    }
}
