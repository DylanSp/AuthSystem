using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IDataAdapter<T> where T : struct, IEntity
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T?> ReadAsync(Guid id);
        Task SaveAsync(T newData);
        Task DeleteAsync(Guid id);
    }
}
