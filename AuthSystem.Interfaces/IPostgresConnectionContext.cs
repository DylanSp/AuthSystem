using Npgsql;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IPostgresConnectionContext : IDisposable
    {
        Task<NpgsqlCommand> CreateCommandAsync();
    }
}
