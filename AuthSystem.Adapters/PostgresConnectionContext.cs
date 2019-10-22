using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using AuthSystem.Interfaces;
using Npgsql;

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

        public async Task OpenAsync()
        {
            if (Connection.State != ConnectionState.Open)
            {
                await Connection.OpenAsync();
            }
        }

        public NpgsqlCommand CreateCommand()
        {
            if (Connection.State != ConnectionState.Open)
            {
                throw new Exception("Attempted to create command on closed connection - check connection opening logic");
            }

            return Connection.CreateCommand();
        }

        public async ValueTask DisposeAsync()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                await Connection.CloseAsync();
            }
        }
    }
}
