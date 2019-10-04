using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IDataAdapter<T> where T : notnull, IEntity
    {
        Task<IEnumerable<T>> ReadAllAsync();
        Task<(bool, T)> ReadAsync(Guid id);
        Task SaveAsync(T newData);
        Task DeleteAsync(Guid id);
    }
}
