using Npgsql;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IPostgresConnectionContext : IAsyncDisposable
    {
        Task OpenAsync();
        NpgsqlCommand CreateCommand();
    }
}
